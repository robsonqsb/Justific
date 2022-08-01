-- cria��o da tabela de usu�rio
create table if not exists usuario (
	id bigserial primary key,
	login varchar(100) not null,
	senha varchar(20) not null,
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean default false
);

-- fun��o para inserir ou alterar um usu�rio
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

-- procedure para excluir logicamente um usu�rio
create or replace procedure p_excluir_usuario (p_id_usuario bigint) as
$$
begin
	update usuario
	set excluido = true,
		alterado_em = current_timestamp
	where id = p_id_usuario;

	assert found, 'Usu�rio com o id ' || p_id_usuario::text || ' n�o foi localizado.';
end;
$$ language plpgsql;

-- view para listagem de usu�rios
create or replace view vw_listar_usuarios as
	select *
		from usuario u
	where not u.excluido;

-- fun��o para confirma��o do login do usu�rio
create or replace function f_confirmar_login_usuario (p_login varchar(100), p_senha varchar(20))
	returns boolean as
$$
	select true
		from vw_listar_usuarios
	where login = trim(p_login) and
		  senha = p_senha;
$$ language sql;

-- fun��o para obter o usu�rio por login
create or replace function f_obter_usuario (p_login varchar(100))
	returns usuario as 
$$
	select *
		from vw_listar_usuarios
	where login = p_login;	
$$ language sql;