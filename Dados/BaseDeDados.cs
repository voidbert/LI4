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

using Dapper;
using Microsoft.Data.SqlClient;

namespace LI4.Dados;

public class BaseDeDados : IBaseDeDados
{
    private static BaseDeDados? _Instancia = null;

    private SqlConnection Conexao;
    private SqlTransaction? TransacaoAtual;
    private int TransacoesAninhadas;

    private BaseDeDados()
    {
        this.Conexao = new SqlConnection("Server=localhost;User Id=WeaponsRUsApp;Password=V3ry$3cur3Pa$$w0rd;Database=WeaponsRUs;Encrypt=false;MultipleActiveResultSets=False");
        this.Conexao.Open();
        this.TransacaoAtual = null;
        this.TransacoesAninhadas = 0;
    }

    public static BaseDeDados Instancia
    {
        get
        {
            if (BaseDeDados._Instancia == null)
                BaseDeDados._Instancia = new BaseDeDados();
            return BaseDeDados._Instancia;
        }
    }

    public List<T> LerDados<T, U>(string sql, U parametros)
    {
        return this.Conexao.Query<T>(sql, parametros, transaction: TransacaoAtual).ToList();
    }

    public void EscreverDados<T>(string sql, T parametros)
    {
        this.Conexao.Execute(sql, parametros, transaction: TransacaoAtual);
    }

    public void IniciarTransacao()
    {
        if (this.TransacaoAtual == null)
        {
            this.TransacaoAtual = this.Conexao.BeginTransaction();
        }

        this.TransacoesAninhadas++;
    }

    public void CommitTransacao()
    {
        if (this.TransacaoAtual != null)
        {
            this.TransacoesAninhadas--;

            if (this.TransacoesAninhadas == 0)
            {
                this.TransacaoAtual.Commit();
                this.TransacaoAtual.Dispose();
                this.TransacaoAtual = null;
            }
        }
    }

    public void AbortarTransacao()
    {
        if (this.TransacaoAtual != null)
        {
            this.TransacaoAtual.Rollback();
            this.TransacaoAtual.Dispose();
            this.TransacaoAtual = null;
            this.TransacoesAninhadas = 0;
        }
    }
}
