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
                        SELECT 
                            id, 
                            name, 
                            email, 
                            admission_date 
                        FROM 
                            Users
                        WHERE 
                            DAY(admission_date) = DAY(GETDATE()) 
                            AND MONTH(admission_date) = MONTH(GETDATE()) 
                            AND YEAR(admission_date) < YEAR(GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime admission_date = reader.GetDateTime(reader.GetOrdinal("admission_date"));
                                int yearsOnCompany = DateTime.Now.Year - admission_date.Year;

                                // Ajuste para aniversários que ainda não ocorreram neste ano
                                if (DateTime.Now.DayOfYear < admission_date.DayOfYear)
                                {
                                    yearsOnCompany--;
                                }

                                // Cria o objeto Colaborador
                                Colaborador colaborador = new Colaborador
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    name = reader.GetString(reader.GetOrdinal("name")),
                                    email = reader.GetString(reader.GetOrdinal("email")),
                                    admission_date = admission_date,
                                    years_on_company = yearsOnCompany
                                };

                                colaboradores.Add(colaborador);
                            }
                        }
                    }

                    // Atualiza a coluna `years_on_company` no banco para cada colaborador
                    foreach (var colaborador in colaboradores)
                    {
                        AtualizarAnosNaEmpresa(colaborador.Id, colaborador.years_on_company);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar aniversariantes: {ex.Message}");
            }

            return colaboradores;
        }

        private void AtualizarAnosNaEmpresa(int id, int yearsOnCompany)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexao.banco))
                {
                    conn.Open();
                    string updateQuery = "UPDATE Users SET years_on_company = @anosNaEmpresa WHERE id = @id";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@anosNaEmpresa", yearsOnCompany);
                        updateCmd.Parameters.AddWithValue("@id", id);

                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar anos na empresa para o colaborador {id}: {ex.Message}");
            }
        }
    }
}
