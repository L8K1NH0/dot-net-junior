CREATE DATABASE DBsistemaCliente

USE DBsistemaCliente

CREATE TABLE Cliente
(
ID INT IDENTITY NOT NULL,
Nome VARCHAR(100) NOT NULL,
CPF_CNPJ VARCHAR(20) NOT NULL,
TipoDocumento VARCHAR(20) NOT NULL

CONSTRAINT PK_ID_cliente PRIMARY KEY (ID),
CONSTRAINT AK_CPF_CNPJ UNIQUE(CPF_CNPJ) 
)

CREATE TABLE Endereco
(
ID INT IDENTITY NOT NULL,
Rua VARCHAR(60) NOT NULL,
Numero VARCHAR(10) NOT NULL,
Cep VARCHAR(30) NOT NULL, 
Bairro VARCHAR(60) NOT NULL,
Cidade VARCHAR(60) NOT NULL,
TipoEndereco VARCHAR(60) NOT NULL,
IDCliente INT NOT NULL  

CONSTRAINT PK_ID_endereco PRIMARY KEY (ID)
CONSTRAINT FK_IDCliente FOREIGN KEY (IDCliente) REFERENCES Cliente (ID)

)

CREATE TABLE Contato
(
ID INT IDENTITY NOT NULL, 
DDD INT NOT NULL,
NumeroContato VARCHAR(20) NOT NULL,
Tipo VARCHAR(20) NOT NULL,
IDCliente INT NOT NULL

CONSTRAINT PK_ID_telCel PRIMARY KEY (ID)
CONSTRAINT FK_IDC_liente FOREIGN KEY (IDCliente) REFERENCES Cliente (ID)
)
