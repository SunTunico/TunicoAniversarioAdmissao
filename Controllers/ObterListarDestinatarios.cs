using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TunicoAniversarioAdmissao.Models;

namespace TunicoAniversarioAdmissao.Controllers
{
    public class DatabaseConnection
    {
        public List<Colaborador> ObterDestinatariosHoje()
        {
            List<Colaborador> colaboradores = new List<Colaborador>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexao.banco))
                {
                    conn.Open();
                    string query = @"
                        SELECT id, nome, email, data_admissao
                        FROM colaboradores
                        WHERE 
                            DAY(data_admissao) = DAY(GETDATE()) 
                            AND MONTH(data_admissao) = MONTH(GETDATE())
                            AND data_admissao <= DATEADD(YEAR, -1, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime dataAdmissao = reader.GetDateTime(reader.GetOrdinal("data_admissao"));
                            int anosNaEmpresa = DateTime.Now.Year - dataAdmissao.Year;

                            // Ajuste para aniversários que ainda não ocorreram neste ano
                            if (DateTime.Now.DayOfYear < dataAdmissao.DayOfYear)
                            {
                                anosNaEmpresa--;
                            }

                            // Cria o objeto Colaborador
                            Colaborador colaborador = new Colaborador
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                DataAdmissao = dataAdmissao,
                                AnosNaEmpresa = anosNaEmpresa
                            };

                            colaboradores.Add(colaborador);
                        }
                    }

                    // Agora, atualiza a coluna anos_na_empresa no banco para cada colaborador
                    foreach (var colaborador in colaboradores)
                    {
                        AtualizarAnosNaEmpresa(conn, colaborador.Id, colaborador.AnosNaEmpresa);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar aniversariantes: {ex.Message}");
            }

            return colaboradores;
        }

        private void AtualizarAnosNaEmpresa(SqlConnection conn, int id, int anosNaEmpresa)
        {
            try
            {
                string updateQuery = "UPDATE colaboradores SET anos_na_empresa = @anosNaEmpresa WHERE id = @id";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@anosNaEmpresa", anosNaEmpresa);
                updateCmd.Parameters.AddWithValue("@id", id);

                updateCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar anos na empresa para o colaborador {id}: {ex.Message}");
            }
        }
    }
}
