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

USE WeaponsRUs;
GO

INSERT INTO Utilizador (EnderecoEletronico, NomeCivil, PalavraPasse, TipoDeConta, PossivelIniciarSessao) VALUES
    ('admin@nerv.jp', 'Humberto Gomes', HASHBYTES('SHA2_512', 'opensource'), 'A', 1),
    ('ayanami@nerv.jp', 'Rei Ayanami', HASHBYTES('SHA2_512', 'ikari-kun'), 'GS', 1),
    ('calvo@nerv.jp', 'Orlando Calvo', HASHBYTES('SHA2_512', 'basededados'), 'GP', 1),
    ('guimaraes@nerv.jp', 'Inês Guimarães', HASHBYTES('SHA2_512', 'helpfrida'), 'GC', 1),
    ('bob@gmail.com', 'Bob', HASHBYTES('SHA2_512', 'bob'), 'C', 1);
GO

INSERT INTO Parte (Nome, Preco, QuantidadeArmazem) VALUES
    ('Cabeça', 200.0, 10),
    ('Tronco', 500.0, 20),
    ('Braço', 100.0, 100),
    ('Perna', 150.0, 100),
    ('Tinta Roxa', 20.0, 10),
    ('Tinta Vermelha', 28.0, 30),
    ('Tinta Branca', 18.0, 5);
GO

INSERT INTO EVA (Nome, Imagem, Preco, QuantidadeArmazem) VALUES
    ('EVA-01', '/eva01.webp', 20000.00, 5),
    ('EVA-02', '/eva02.webp', 22000.00, 5),
    ('EVA-03', '/eva03.webp', 25000.00, 3);
GO

INSERT INTO EVAPartes (EVA, Parte, Quantidade) VALUES
    (1, 1, 1),
    (1, 2, 1),
    (1, 3, 2),
    (1, 4, 2),
    (1, 5, 1),

    (2, 1, 1),
    (2, 2, 1),
    (2, 3, 2),
    (2, 4, 2),
    (2, 6, 1),

    (3, 1, 1),
    (3, 2, 1),
    (3, 3, 2),
    (3, 4, 2),
    (3, 7, 1);
GO
