-- criação da tabela de justificativa
create table if not exists justificativa
(
	id bigserial primary key,
	membro_id bigint not null references membro(id),
	data_ocorrencia date not null default current_date,
	possui_comprovante boolean not null default true,
	comentarios varchar(1000),
	data_criacao timestamp not null default current_timestamp,
	alterado_em timestamp default null,
	excluido boolean not null default false
);

-- função para incluir/alterar uma justificativa
create or replace function f_incluir_alterar_justificativa (p_codigo_registro_membro varchar(50), 
															p_cnpj_organizacao varchar(20),
															p_comentarios varchar(1000),
															p_data_ocorrencia date default current_date, 
															p_possui_comprovante boolean default true)
	returns bigint as
$$
declare
	_id_membro bigint;
	_id_justificativa bigint;
begin
	
	assert f_validar_cnpj(p_cnpj_organizacao), 'O CNPJ da organização é inválido';
		
	select m.membroid into _id_membro
		from vw_listar_membros m
			inner join vw_listar_organizacoes o
				on m.organizacaoid = o.id
	where m.codigoregistro = p_codigo_registro_membro and
		  o.cnpj = f_considerar_somente_digitos(p_cnpj_organizacao);

	assert found, 'O membro com o código de registros ' || p_codigo_registro_membro || ' e CNPJ ' || p_cnpj_organizacao || ' não foi localizado.';

	select justificativa_id into _id_justificativa
		from vw_listar_justificativas
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
create or replace procedure p_excluir_justificativa (p_id_justificativa bigint) as
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
		   m.CodigoRegistro,
		   m.NomeMembro,
		   m.OrganizacaoId,
		   o.nome nome_organizacao,
		   o.cnpj
		from justificativa j
			inner join vw_listar_membros m
				on j.membro_id = m.MembroId
			inner join vw_listar_organizacoes o
				on m.OrganizacaoId = o.id
	where not j.excluido;

-- criação da função para obter a justificativa
create or replace function f_obter_justificativa (p_codigo_registro_membro varchar(50),
											      p_cnpj_organizacao char(14),
												  p_data_ocorrencia date)
	returns setof vw_listar_justificativas as
$$
	select *
		from vw_listar_justificativas lj
	where lj.codigoregistro = p_codigo_registro_membro
		and lj.cnpj = p_cnpj_organizacao
		and lj.data_ocorrencia = p_data_ocorrencia;	
$$ language sql;
