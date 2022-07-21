-- criação da tabela de justificativa
create table if not exists justificativa
(
	id serial primary key,
	membro_id int not null references membro(id),
	data_ocorrencia date not null default current_date,
	possui_comprovante boolean default true,
	comentarios varchar(1000),
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean not null default false
);

-- função para incluir/alterar uma justificativa
create or replace function f_incluir_alterar_justificativa (p_codigo_registro_membro varchar(50), 
															p_cnpj_organizacao varchar(12),
															p_comentarios varchar(1000),
															p_data_ocorrencia date default current_date, 
															p_possui_comprovante boolean default true)
	returns int as
$$
declare
	_id_membro int;
	_id_justificativa int;
begin
	select m.membro_id into _id_membro
		from vw_listar_membros m
			inner join vw_listar_organizacoes o
				on m.organizacao_id = o.id
	where m.codigo_registro = p_codigo_registro_membro and
		  o.cnpj = p_cnpj_organizacao;

	assert found, 'O membro com o código de registros ' || p_codigo_registro_membro || ' e CNPJ ' || p_cnpj_organizacao || ' não foi localizado.';

	select id into _id_justificativa
		from justificativa
	where membro_id = _id_membro and
		data_ocorrencia = p_data_ocorrencia;
	
	if found then
		update justificativa
		set possui_comprovante = p_possui_comprovante,
			comentarios = p_comentarios,
			alterado_em = current_timestamp 
		where id = _id_justificativa;
		
		return _id_justificativa;
	end if;

	insert into justificativa (membro_id, data_ocorrencia, possui_comprovante, comentarios)
	values (_id_membro, p_data_ocorrencia, p_possui_comprovante, p_comentarios)
	returning id into _id_justificativa;
	
	return _id_justificativa;
end;
$$ language plpgsql;

-- procedure para excluir logicamente uma justificativa
create or replace procedure p_excluir_justificativa (p_id_justificativa int) as
$$
begin
	update justificativa
	set excluido = true,
		alterado_em = current_timestamp 
	where id = p_id_justificativa;

	assert found, 'Justificativa com id ' || p_id_justificativa || ' não localizada.';
end;
$$ language plpgsql;

-- criação da view para a listagem de justificativas
create or replace view vw_listar_justificativas as
	select j.id justificativa_id,
		   j.data_ocorrencia,
		   j.possui_comprovante,
		   j.comentarios,
		   j.data_criacao,
		   j.alterado_em,
		   j.membro_id,
		   m.codigo_registro,
		   m.nome_membro,
		   m.organizacao_id,
		   o.nome nome_organizacao,
		   o.cnpj
		from justificativa j
			inner join vw_listar_membros m
				on j.membro_id = m.membro_id
			inner join vw_listar_organizacoes o
				on m.organizacao_id = o.id
	where not j.excluido;
