-- exclusão da tabela de usuário
drop table if exists usuario;

-- criação da tabela de usuário
create table usuario (
	id serial primary key,
	login varchar(100) not null,
	senha varchar(20) not null,
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean default false
);

-- função para inserir ou alterar um usuário
create or replace function f_incluir_alterar_usuario (p_login varchar(100), p_senha varchar(20))
	returns bigint as
$$	
declare
	_id_usuario bigint;
begin
	select id into _id_usuario
		from vw_listar_usuarios
	where login = trim(p_login);

	if found then	
		update usuario
		set senha = p_senha,
			alterado_em = current_timestamp
		where id = _id_usuario;	
		return _id_usuario;
	end if;

	insert into usuario (login, senha)
		values (p_login, p_senha)
	returning id into _id_usuario;

	return _id_usuario;
end;
$$ language plpgsql;

-- função para excluir logicamente um usuário
create or replace procedure p_excluir_usuario (p_id_usuario bigint) as
$$
begin
	update usuario
	set excluido = true,
		alterado_em = current_timestamp
	where id = p_id_usuario;

	assert found, 'Usuário com o id ' || p_id_usuario::text || ' não foi localizado.';
end;
$$ language plpgsql;

-- view para listagem de usuários
create or replace view vw_listar_usuarios as
	select *
		from usuario u
	where not u.excluido;

-- função para confirmação do login do usuário
create or replace function f_confirmar_login_usuario (p_login varchar(100), p_senha varchar(20))
	returns boolean as
$$
	select true
		from vw_listar_usuarios
	where login = trim(p_login) and
		  senha = p_senha;
$$ language sql;

-- função para obter o usuário por login
create or replace function f_obter_usuario (p_login varchar(100))
	returns usuario as 
$$
	select *
		from vw_listar_usuarios
	where login = p_login;	
$$ language sql;