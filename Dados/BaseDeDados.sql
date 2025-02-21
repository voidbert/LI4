-- Copyright 2025 Ana Cerqueira, Humberto Gomes, João Torres, José Lopes, José Matos
--
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
--
--    http://www.apache.org/licenses/LICENSE-2.0
--
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.

DROP DATABASE IF EXISTS WeaponsRUs;
GO
CREATE DATABASE WeaponsRUs;
GO
USE WeaponsRUs;
GO

CREATE TABLE Utilizador (
  EnderecoEletronico    VARCHAR(75)                  NOT NULL,
  NomeCivil             VARCHAR(125)                 NOT NULL,
  PalavraPasse          VARBINARY(64)                NOT NULL,
  TipoDeConta           VARCHAR(2)                   NOT NULL,
  PossivelIniciarSessao BIT           DEFAULT 'TRUE' NOT NULL,

  CONSTRAINT UtilizadorPK          PRIMARY KEY (EnderecoEletronico),
  CONSTRAINT UtilizadorTipoDeConta CHECK       (TipoDeConta IN ('C', 'A', 'GS', 'GP', 'GC'))
);
GO

CREATE FUNCTION UtilizadorE (@id VARCHAR(75), @tipo VARCHAR(2)) RETURNS BIT AS BEGIN
    IF EXISTS (SELECT * FROM Utilizador WHERE EnderecoEletronico = @id AND TipoDeConta = @tipo)
        RETURN 1;
    RETURN 0;
END
GO

CREATE TABLE EVA (
  Identificador     INT IDENTITY           NOT NULL,
  Nome              VARCHAR(75)            NOT NULL,
  Imagem            VARCHAR(125)           NOT NULL,
  Preco             MONEY                  NOT NULL,
  QuantidadeArmazem INT          DEFAULT 0 NOT NULL,

  CONSTRAINT EVAPK                    PRIMARY KEY (Identificador),
  CONSTRAINT EVAPrecoNaoNegativo      CHECK       (Preco >= 0),
  CONSTRAINT EVAQuantidadeNaoNegativa CHECK       (QuantidadeArmazem >= 0)
);
GO

CREATE TABLE CarrinhoCompras (
  Cliente    VARCHAR(75) NOT NULL,
  EVA        INT         NOT NULL,
  Quantidade INT         NOT NULL,

  CONSTRAINT CarrinhoComprasPK                 PRIMARY KEY (Cliente, EVA),
  CONSTRAINT CarrinhoComprasFKUtilizador       FOREIGN KEY (Cliente)                           REFERENCES Utilizador (EnderecoEletronico),
  CONSTRAINT CarrinhoComprasFKEVA              FOREIGN KEY (EVA)                               REFERENCES EVA        (Identificador),
  CONSTRAINT CarrinhoComprasQuantidadePositiva CHECK       (Quantidade > 0),
  CONSTRAINT CarrinhoComprasUtilizadorCliente  CHECK       (dbo.UtilizadorE(Cliente, 'C') = 1)
);
GO

CREATE TABLE EncomendaEVAs (
  Identificador        INT IDENTITY                 NOT NULL,
  Cliente              VARCHAR(75)                  NOT NULL,
  Morada               VARCHAR(125)                 NOT NULL,
  Preco                MONEY                        NOT NULL,
  InstanteColocacao    DATETIME                     NOT NULL,
  InstanteConfirmacao  DATETIME                     NULL,
  InstanteEntrega      DATETIME                     NULL,
  InstanteCancelamento DATETIME                     NULL,
  InstanteDevolucao    DATETIME                     NULL,
  Aprovada             BIT          DEFAULT 'FALSE' NOT NULL,

  CONSTRAINT EncomendaEVAsPK                PRIMARY KEY (Identificador),
  CONSTRAINT EncomendaEVAsFKCliente         FOREIGN KEY (Cliente)                            REFERENCES Utilizador (EnderecoEletronico),
  CONSTRAINT EncomendaEVAsUtilizadorCliente CHECK       (dbo.UtilizadorE(Cliente, 'C') = 1),
  CONSTRAINT EncomendaEVAsPrecoNaoNegativo  CHECK       (Preco >= 0),

  CONSTRAINT EncomendaEVAsOrdemDatas1 CHECK (InstanteConfirmacao  IS NULL OR InstanteColocacao    < InstanteConfirmacao),
  CONSTRAINT EncomendaEVAsOrdemDatas2 CHECK (InstanteEntrega      IS NULL OR InstanteConfirmacao  < InstanteEntrega),
  CONSTRAINT EncomendaEVAsOrdemDatas3 CHECK (InstanteCancelamento IS NULL OR InstanteColocacao    < InstanteCancelamento),
  CONSTRAINT EncomendaEVAsOrdemDatas4 CHECK (InstanteCancelamento IS NULL OR InstanteCancelamento < InstanteEntrega),
  CONSTRAINT EncomendaEVAsOrdemDatas5 CHECK (InstanteDevolucao    IS NULL OR InstanteEntrega      < InstanteDevolucao),

  CONSTRAINT EncomendaEVAsDatasObrigatorias1 CHECK (Aprovada = 0 OR InstanteConfirmacao IS NOT NULL),
  CONSTRAINT EncomendaEVAsDatasObrigatorias2 CHECK (InstanteEntrega IS NULL OR Aprovada = 1),
  CONSTRAINT EncomendaEVAsDatasObrigatorias3 CHECK (InstanteDevolucao IS NULL OR InstanteEntrega IS NOT NULL),
  CONSTRAINT EncomendaEVAsDatasObrigatorias4 CHECK (InstanteEntrega IS NULL OR InstanteCancelamento IS NULL)
);
GO

CREATE TABLE ConteudoEncomendaEVAs (
  Encomenda  INT NOT NULL,
  EVA        INT NOT NULL,
  Quantidade INT NOT NULL,

  CONSTRAINT ConteudoEncomendaEVAsPK                 PRIMARY KEY (Encomenda, EVA),
  CONSTRAINT ConteudoEncomendaEVAsFKEncomenda        FOREIGN KEY (Encomenda)       REFERENCES EncomendaEVAs (Identificador),
  CONSTRAINT ConteudoEncomendaEVAsFKEVA              FOREIGN KEY (EVA)             REFERENCES EVA           (Identificador),
  CONSTRAINT ConteudoEncomendaEVAsQuantidadePositiva CHECK       (Quantidade > 0)
);
GO

CREATE TABLE OrdemProducao (
  Identificador   INT IDENTITY                 NOT NULL,
  Funcionario     VARCHAR(75)                  NOT NULL,
  InstanteEmissao DATETIME                     NOT NULL,
  Visualizada     BIT          DEFAULT 'FALSE' NOT NULL,

  CONSTRAINT OrdemProducaoPK                PRIMARY KEY (Identificador),
  CONSTRAINT OrdemProducaoFKFuncionario     FOREIGN KEY (Funcionario)                             REFERENCES Utilizador (EnderecoEletronico),
  CONSTRAINT OrdemProducaoUtilizadorCliente CHECK       (dbo.UtilizadorE(Funcionario, 'GP') = 1),
);
GO

CREATE TABLE ConteudoOrdemProducao (
  OrdemProducao INT NOT NULL,
  EVA           INT NOT NULL,
  Quantidade    INT NOT NULL,

  CONSTRAINT ConteudoOrdemProducaoPK                 PRIMARY KEY (OrdemProducao, EVA),
  CONSTRAINT ConteudoOrdemProducaoFKOrdemProducao    FOREIGN KEY (OrdemProducao)       REFERENCES OrdemProducao (Identificador),
  CONSTRAINT ConteudoOrdemProducaoFKEVA              FOREIGN KEY (EVA)                 REFERENCES EVA (Identificador),
  CONSTRAINT ConteudoOrdemProducaoQuantidadePositiva CHECK       (Quantidade > 0)
);
GO

CREATE TABLE Parte (
  Identificador     INT IDENTITY           NOT NULL,
  Nome              VARCHAR(75)            NOT NULL,
  Preco             MONEY                  NOT NULL,
  QuantidadeArmazem INT          DEFAULT 0 NOT NULL,

  CONSTRAINT PartePK                    PRIMARY KEY (Identificador),
  CONSTRAINT PartePrecoNaoNegativo      CHECK       (Preco >= 0),
  CONSTRAINT ParteQuantidadeNaoNegativa CHECK       (QuantidadeArmazem >= 0)
);
GO

CREATE TABLE EVAPartes (
  EVA        INT NOT NULL,
  Parte      INT NOT NULL,
  Quantidade INT NOT NULL,

  CONSTRAINT EVAPartesPK                 PRIMARY KEY (EVA, Parte),
  CONSTRAINT EVAPartesFKEVA              FOREIGN KEY (EVA)            REFERENCES EVA   (Identificador),
  CONSTRAINT EVAPartesFKParte            FOREIGN KEY (Parte)          REFERENCES Parte (Identificador),
  CONSTRAINT EVAPartesQuantidadePositiva CHECK       (Quantidade > 0)
);
GO

CREATE TABLE EncomendaPartes (
  Identificador      INT IDENTITY NOT NULL,
  InstanteRealizacao DATETIME     NOT NULL,
  Preco              MONEY        NOT NULL,
  Funcionario        VARCHAR(75)  NOT NULL,

  CONSTRAINT EncomendaPartesPK                      PRIMARY KEY (Identificador),
  CONSTRAINT EncomendaPartesFKFuncionario           FOREIGN KEY (Funcionario)                            REFERENCES Utilizador (EnderecoEletronico),
  CONSTRAINT EncomendaPartesPrecoNaoNegativo        CHECK       (Preco >= 0),
  CONSTRAINT EncomendaPartesUtilizadorGestorDeStock CHECK       (dbo.UtilizadorE(Funcionario, 'GS') = 1)
);
GO

CREATE TABLE ConteudoEncomendaPartes (
  Encomenda  INT NOT NULL,
  Parte      INT NOT NULL,
  Quantidade INT NOT NULL,

  CONSTRAINT ConteudoEncomendaPartesPK                 PRIMARY KEY (Encomenda, Parte),
  CONSTRAINT ConteudoEncomendaPartesFKEncomenda        FOREIGN KEY (Encomenda)         REFERENCES EncomendaPartes (Identificador),
  CONSTRAINT ConteudoEncomendaPartesFKParte            FOREIGN KEY (Parte)             REFERENCES Parte           (Identificador),
  CONSTRAINT ConteudoEncomendaPartesQuantidadePositiva CHECK       (Quantidade > 0)
);
GO

CREATE LOGIN WeaponsRUsApp WITH PASSWORD = 'V3ry$3cur3Pa$$w0rd';
GO

CREATE USER WeaponsRUsApp FOR LOGIN WeaponsRUsApp;
GO

GRANT SELECT, INSERT, UPDATE ON Utilizador              TO WeaponsRUsApp;
GRANT SELECT, UPDATE         ON EVA                     TO WeaponsRUsApp;
GRANT SELECT, INSERT, DELETE ON CarrinhoCompras         TO WeaponsRUsApp;
GRANT SELECT, INSERT, UPDATE ON EncomendaEVAs           TO WeaponsRUsApp;
GRANT SELECT, INSERT, DELETE ON ConteudoEncomendaEVAs   TO WeaponsRUsApp;
GRANT SELECT, INSERT, UPDATE ON OrdemProducao           TO WeaponsRUsApp;
GRANT SELECT, INSERT, DELETE ON ConteudoOrdemProducao   TO WeaponsRUsApp;
GRANT SELECT, UPDATE         ON Parte                   TO WeaponsRUsApp;
GRANT SELECT, INSERT, DELETE ON EVAPartes               TO WeaponsRUsApp;
GRANT SELECT, INSERT         ON EncomendaPartes         TO WeaponsRUsApp;
GRANT SELECT, INSERT         ON ConteudoEncomendaPartes TO WeaponsRUsApp;
GO
