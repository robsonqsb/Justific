-- criação da tabela de organização
drop table if exists organizacao;
create table organizacao
(
	id serial primary key,
	nome varchar(500) not null,
	cnpj char(14) not null,
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean not null default false
);

-- criação da tabela de relação entre usuários e organizações
drop table if exists usuario_organizacao;
create table usuario_organizacao
(
	usuario_id bigint not null references usuario(id) on delete cascade,
	organizacao_id bigint not null references organizacao(id) on delete cascade,
	excluido boolean not null default false,
	constraint usuario_organizacao_pkey primary key (usuario_id, organizacao_id)
);

-- criação da função para validar o cnpj informado
create or replace function f_validar_cnpj (p_cnpj char(14))
	returns boolean as
$$
begin
	return char_length(trim(replace(replace(replace(p_cnpj, '.', ''), '/', ''), '-', ''))) = 14;	
end;
$$ language plpgsql;


-- função para incluir/alterar organização
create or replace function f_incluir_alterar_organizacao(p_nome varchar(500), p_cnpj char(14))
	returns bigint as
$$
	declare
		_id_organizacao bigint;
	begin
		assert (select f_validar_cnpj(p_cnpj)), 'CNPJ no formato inválido';
	
		select id into _id_organizacao
			from organizacao
		where cnpj = p_cnpj;
	
		if found then
			update organizacao
			set nome = p_nome,
				alterado_em = current_timestamp
			where id = _id_organizacao;
			return _id_organizacao;
		end if;
	
		insert into organizacao (nome, cnpj)
		values (p_nome, p_cnpj)
		returning id into _id_organizacao;
	
		return _id_organizacao;
	end;
$$ language plpgsql;

-- procedure para excluir logicamente uma organização
create or replace procedure p_excluir_organizacao(p_id_organizacao bigint) as
$$
begin
	update usuario_organizacao 
	set excluido = true
	where organizacao_id = p_id_organizacao;
	
	update organizacao
	set excluido = true,
		alterado_em = current_timestamp
	where id = p_id_organizacao;

	assert found, 'Organização com o id ' || p_id_organizacao::text || ' não foi localizado.';
end;		
$$ language plpgsql;

-- view para listagem de organizações
create or replace view vw_listar_organizacoes as
	select *
		from organizacao o
	where not o.excluido;

-- função para obter a organização por cnpj
create or replace function f_obter_organizacao (p_cnpj char(14))
	returns organizacao as
$$
	select *
		from vw_listar_organizacoes
	where cnpj = trim(replace(replace(replace(p_cnpj, '.', ''), '/', ''), '-', '')) 
$$ language sql;


-- criação da função para vincular organização ao usuário
create or replace function f_vincular_organizacao_usuario (p_login_usuario varchar(100), p_cnpj_organizacao char(14), p_desfazer boolean default false)
	returns boolean as
$$
declare
	_id_usuario bigint;
	_id_organizacao bigint;
	_existe_registro_excluido boolean;
begin
	select id into _id_usuario
		from vw_listar_usuarios
	where login = trim(p_login_usuario);

	assert found, concat('Usuário com o login ', p_login_usuario, ' não foi localizado');

	select id into _id_organizacao
		from vw_listar_organizacoes
	where cnpj = trim(replace(replace(replace(p_cnpj_organizacao, '.', ''), '/', ''), '-', ''));

	assert found, concat('A organização não foi localizada pelo cnpj');

	select excluido into _existe_registro_excluido
		from usuario_organizacao
	where usuario_id = _id_usuario and
		  organizacao_id = _id_organizacao;
		 
	if found and _existe_registro_excluido and not p_desfazer then
		update usuario_organizacao
		set excluido = false	
		where usuario_id = _id_usuario and
			  organizacao_id = _id_organizacao;
		return true;
	elseif found and not _existe_registro_excluido and p_desfazer then
		update usuario_organizacao
		set excluido = true					
		where usuario_id = _id_usuario and
			  organizacao_id = _id_organizacao;
		return true;		
	elseif found then
		return true;
	end if;
	
	insert into usuario_organizacao (usuario_id, organizacao_id)
		values (_id_usuario, _id_organizacao);
	
	return true;
end;
$$ language plpgsql;

-- criação da função para listar as associações entre organizações e usuários
create or replace function f_listar_organizacoes_usuarios(p_cnpj_organizacao char(14))
	returns table (organizacao_id int, nome_organizacao varchar(500), usuario_id int, login_usuario varchar(100)) as
$$
begin
	assert (select f_validar_cnpj(p_cnpj)), 'CNPJ no formato inválido';
	return query
		select o.id,
			   o.nome,
			   u.id,
			   u.login
			from vw_listar_organizacoes o 
				inner join usuario_organizacao uo 
					on o.id = uo.organizacao_id 
				inner join vw_listar_usuarios u 
					on uo.usuario_id = u.id
		where o.cnpj = p_cnpj_organizacao;
end;
$$ language plpgsql;