-- criação da tabela de membro
create table if not exists membro
(
	id bigserial primary key,
	codigo_registro varchar(50) not null,
	nome varchar(500) not null,
	organizacao_id bigint not null references organizacao(id),
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean not null default false,
	unique(organizacao_id, codigo_registro)
);

-- função para incluir/ alterar um membro
create or replace function f_incluir_alterar_membro (p_codigo_registro varchar(50), p_nome varchar(500), p_cnpj_organizacao char(20))
	returns bigint as 
$$
	declare
		_id_organizacao bigint;
		_id_membro bigint;		
	begin
		assert char_length(p_codigo_registro) > 0, 'O código de registro deve ser informado';
		assert f_validar_cnpj(p_cnpj_organizacao), 'CNPJ possui um formato inválido';
				
		select id into _id_organizacao
			from vw_listar_organizacoes
		where cnpj = f_considerar_somente_digitos(p_cnpj_organizacao);
	
		assert found, 'Organização com o cnpj ' || p_cnpj_organizacao || ' não foi localizada';
	
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

-- procedure para a exclusão de um membro
create or replace procedure p_excluir_membro(membro_id bigint) as
$$
	begin
		update membro 
		set excluido = true,
			alterado_em = current_timestamp
		where id = membro_id;
	end;
$$ language plpgsql;

-- view para listar os membros e organização atrelada
create or replace view vw_listar_membros as
	select m.id MembroId,
		   m.codigo_registro CodigoRegistro,
		   m.nome NomeMembro,
		   m.data_criacao DataCriacaoMembro,
		   m.alterado_em MembroAlteradoEm,
		   o.id OrganizacaoId,
		   o.nome NomeOrganizacao,
		   o.cnpj
		from membro m
			inner join vw_listar_organizacoes o
				on m.organizacao_id = o.id
	where not m.excluido;

-- função para obter um membro através do código de registro
create or replace function f_obter_membro(p_codigo_registro varchar(50), p_organizacao_id bigint)
	returns setof vw_listar_membros as
$$			
	select *
		from vw_listar_membros
	where OrganizacaoId = p_organizacao_id and
		CodigoRegistro = p_codigo_registro;	
$$ language sql;
