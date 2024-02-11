db.createCollection("usuario",
{
    validator: {
        $jsonSchema: {
            bsonType: "object",
            additionalProperties: false,
            required: ["_id", "login", "senha", "data_criacao", "excluido"],
            properties: {
                "_id" : {
                  bsonType: "objectId"
                },
                login: {
                    bsonType: "string",
                    description: "informe o login",
                    maxLength: 100
                },
                senha:{
                    bsonType: "string",
                    description: "informe a senha",
                    maxLength: 20
                },
                data_criacao: {
                    bsonType: "date",
                    description: "informe a data de criação do registro"
                },
                alterado_em: {
                    bsonType : ["date", "null"]
                },
                excluido : {
                    bsonType: "bool",
                    description: "informe se o registro é excluído"
                }
            }
        }
    }    
})