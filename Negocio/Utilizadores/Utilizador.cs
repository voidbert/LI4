/*
 * Copyright 2025 Ana Cerqueira, Humberto Gomes, João Torres, José Lopes, José Matos
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public abstract class Utilizador
{
    public enum Tipo
    {
        Cliente,
        Administrador,
        GestorDeStock,
        GestorDeProducao,
        GestorDeContas
    }

    protected Utilizador(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao)
    {
        this.EnderecoEletronico = enderecoEletronico;
        this.NomeCivil = nomeCivil;
        this.PalavraPasse = palavraPasse;
        this.PossivelIniciarSessao = possivelIniciarSessao;
    }

    protected Utilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao)
    {
        this.EnderecoEletronico = enderecoEletronico;
        this.NomeCivil = nomeCivil;
        this.PalavraPasse = Utilizador.HashDaPalavraPasse(palavraPasse);
        this.PossivelIniciarSessao = possivelIniciarSessao;
    }

    public bool PalavraPasseCorreta(string palavraPasse)
    {
        byte[] hash = Utilizador.HashDaPalavraPasse(palavraPasse);
        return Enumerable.SequenceEqual(hash, this.PalavraPasse);
    }

    public static byte[] HashDaPalavraPasse(string palavraPasse)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(palavraPasse);
        return SHA512.Create().ComputeHash(bytes);
    }

    public static Tipo TipoDeString(string tipo)
    {
        Dictionary<string, Tipo> tipos = new Dictionary<string, Tipo> {
            {"C", Tipo.Cliente},
            {"A", Tipo.Administrador},
            {"GS", Tipo.GestorDeStock},
            {"GP", Tipo.GestorDeProducao},
            {"GC", Tipo.GestorDeContas}
        };

        if (!tipos.ContainsKey(tipo))
        {
            throw new TipoDeContaInexistenteException();
        }
        return tipos[tipo];
    }

    public static string StringDeTipo(Tipo tipo)
    {
        Dictionary<Tipo, string> strings = new Dictionary<Tipo, string> {
            {Tipo.Cliente, "C"},
            {Tipo.Administrador, "A"},
            {Tipo.GestorDeStock, "GS"},
            {Tipo.GestorDeProducao, "GP"},
            {Tipo.GestorDeContas, "GC"}
        };

        return strings[tipo];
    }

    public UtilizadorModel ParaModel()
    {
        return new UtilizadorModel
        {
            EnderecoEletronico = this.EnderecoEletronico,
            NomeCivil = this.NomeCivil,
            PalavraPasse = this.PalavraPasse,
            TipoDeConta = Utilizador.StringDeTipo(this.TipoDeConta),
            PossivelIniciarSessao = this.PossivelIniciarSessao,
            Encomendas = (this is Cliente) ? ((Cliente)this).EncomendasRaw : null
        };
    }

    public static Utilizador DeModel(UtilizadorModel model)
    {
        Tipo tipo = Utilizador.TipoDeString(model.TipoDeConta);
        switch (tipo)
        {
            case Tipo.Cliente:
                return new Cliente(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao, model.Encomendas!);
            case Tipo.Administrador:
                return new Administrador(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.GestorDeStock:
                return new GestorDeStock(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.GestorDeProducao:
                return new GestorDeProducao(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao, model.OrdensProducao!);
            case Tipo.GestorDeContas:
            default:
                return new GestorDeContas(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
        }
    }

    public static Utilizador Criar(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao, Tipo tipoDeConta)
    {
        switch (tipoDeConta)
        {
            case Tipo.Cliente:
                return new Cliente(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao, new List<int>());
            case Tipo.Administrador:
                return new Administrador(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao);
            case Tipo.GestorDeStock:
                return new GestorDeStock(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao);
            case Tipo.GestorDeProducao:
                return new GestorDeProducao(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao, new List<int>());
            case Tipo.GestorDeContas:
            default:
                return new GestorDeContas(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao);
        }
    }

    public override int GetHashCode()
    {
        return this.EnderecoEletronico.GetHashCode();
    }

    public string EnderecoEletronico { get; init; }
    public string NomeCivil { get; set; }
    public byte[] PalavraPasse { get; set; }
    public abstract Tipo TipoDeConta { get; }
    public bool PossivelIniciarSessao { get; set; }
}
