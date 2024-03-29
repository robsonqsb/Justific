-- cria��o da tabela de membro
create table if not exists membro
(
	id serial primary key,
	codigo_registro varchar(50) not null,
	nome varchar(500) not null,
	organizacao_id int not null references organizacao(id),
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp,
	excluido boolean not null default false,
	unique(organizacao_id, codigo_registro)
);

-- fun��o para incluir/ alterar um membro
create or replace function f_incluir_alterar_membro (p_codigo_registro varchar(50), p_nome varchar(500), p_cnpj_organizacao char(14))
	returns int as 
$$
	declare
		_id_organizacao int;
		_id_membro int;		
	begin
		assert char_length(p_codigo_registro) > 0, 'O c�digo de registro deve ser informado';
		assert (select f_validar_cnpj(p_cnpj_organizacao)), 'CNPJ possui o formato inv�lido';
				
		select id into _id_organizacao
			from vw_listar_organizacoes
		where cnpj = trim(replace(replace(replace(p_cnpj_organizacao, '.', ''), '/', ''), '-', ''));
	
		assert found, 'Organiza��o com o cnpj ' || p_cnpj_organizacao || ' n�o foi localizada';
	
		select id into _id_membro
			from membro
		where codigo_registro = p_codigo_registro and
			  organizacao_id = _id_organizacao;
	
		if found then
			update membro
			set nome = p_nome,
				alterado_em = current_timestamp 
			where id = _id_membro;
			return _id_membro;
		end if;
	
		insert into membro (codigo_registro, nome, organizacao_id)
			values (p_codigo_registro, p_nome, _id_organizacao)
		returning id into _id_membro;
	
		return _id_membro;
	end;
$$ language plpgsql;

-- procedure para a exclus�o de um membro
create or replace procedure p_excluir_membro(membro_id int) as
$$
	begin
		update membro 
		set excluido = true,
			alterado_em = current_timestamp
		where id = membro_id;
	end;
$$ language plpgsql;

-- view para listar os membros e organiza��o atrelada
create or replace view vw_listar_membros as
	select *
		from membro m
			inner join vw_listar_organizacoes o
				on m.organizacao_id = o.id
	where not m.excluido;

-- fun��o para obter um membro atrav�s do c�digo de registro
create or replace function f_obter_membro(p_codigo_registro varchar(50), p_organizacao_id int)
	returns membro as
$$			
	select *
		from vw_listar_membros
	where organizacao_id = p_organizacao_id and
		codigo_registro = p_codigo_registro;	
$$ language sql;
