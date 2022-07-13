-- criação da tabela de funcionário
create table if not exists funcionario
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

-- função para incluir/ alterar um funcionário
create or replace function f_incluir_alterar_funcionario (p_codigo_registro varchar(50), p_nome varchar(500), p_cnpj_organizacao char(14))
	returns int as 
$$
	declare
		_id_organizacao int;
		_id_funcionario int;		
	begin
		assert char_length(p_codigo_registro) > 0, 'O código de registro deve ser informado';
		assert (select f_validar_cnpj(p_cnpj_organizacao)), 'CNPJ possui o formato inválido';
				
		select id into _id_organizacao
			from vw_listar_organizacoes
		where cnpj = trim(replace(replace(replace(p_cnpj_organizacao, '.', ''), '/', ''), '-', ''));
	
		assert found, 'Organização com o cnpj ' || p_cnpj_organizacao || ' não foi localizada';
	
		select id into _id_funcionario
			from funcionario
		where codigo_registro = p_codigo_registro and
			  organizacao_id = _id_organizacao;
	
		if found then
			update funcionario
			set nome = p_nome,
				alterado_em = current_timestamp 
			where id = _id_funcionario;
			return _id_funcionario;
		end if;
	
		insert into funcionario (codigo_registro, nome, organizacao_id)
			values (p_codigo_registro, p_nome, _id_organizacao)
		returning id into _id_funcionario;
	
		return _id_funcionario;
	end;
$$ language plpgsql;

-- procedure para a exclusão de um funcionário
create or replace procedure p_excluir_funcionario(funcionario_id int) as
$$
	begin
		update funcionario 
		set excluido = true,
			alterado_em = current_timestamp
		where id = funcionario_id;
	end;
$$ language plpgsql;

-- view para listar os funcionários
create or replace view vw_listar_funcionarios as
	select *
		from funcionario
	where not excluido;

-- função para obter um funcionário através do código de registro
create or replace function f_obter_funcionario(p_codigo_registro varchar(50), p_organizacao_id int)
	returns funcionario as
$$			
	select *
		from vw_listar_funcionarios
	where organizacao_id = p_organizacao_id and
		codigo_registro = p_codigo_registro;	
$$ language sql;
