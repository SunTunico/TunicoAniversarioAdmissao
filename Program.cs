using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TunicoAniversarioAdmissao.Controllers;
using TunicoAniversarioAdmissao.Models;
using TunicoAniversarioAdmissao.Services;

namespace TunicoAniversarioAdmissao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando rotina de anivers�rios de admiss�o...");

            //while (true)
            //{
               
                    ExecutarRotinaAniversarios();
                //}

                //Thread.Sleep(60000);
        //    }
        }

        static void ExecutarRotinaAniversarios()
        {
            Console.WriteLine("Executando rotina de anivers�rios de admiss�o...");

            DatabaseConnection dbConnection = new DatabaseConnection();
            EmailService emailService = new EmailService();
            List<Colaborador> aniversariantes;

            try
            {
                aniversariantes = dbConnection.ObterDestinatariosHoje();

                if (aniversariantes.Count > 0)
                {
                    // Envia e-mails de anivers�rio para cada colaborador
                    foreach (var colaborador in aniversariantes)
                    {
                        emailService.EnviarEmailAniversario(colaborador);
                    }

                    // Envia notifica��o com lista de destinat�rios
                    string listaDestinatarios = string.Join(", ", aniversariantes.Select(c => $"{c.name} ({c.email})"));
                    emailService.EnviarEmailNotificacao("Rotina Executada com Sucesso",
                        $"A rotina de anivers�rios de admiss�o foi executada com sucesso. Destinat�rios:\n\n{listaDestinatarios}");

                    Console.WriteLine("E-mails enviados e notifica��o enviada.");
                    Console.WriteLine("E-mails enviados e notifica��o enviada.");
                    Console.WriteLine("E-mails enviados e notifica��o enviada.");
                }
                else
                {
                    // Envia notifica��o informando que n�o h� aniversariantes
                    emailService.EnviarEmailNotificacao("Rotina Executada - Sem Destinat�rios",
                        "A rotina de anivers�rios de admiss�o foi executada, mas n�o havia aniversariantes hoje.");

                    Console.WriteLine("Nenhum colaborador com anivers�rio de admiss�o hoje. Notifica��o enviada.");
                }
            }
            catch (Exception ex)
            {
                // Envia notifica��o de erro
                emailService.EnviarEmailNotificacao("Erro na Rotina de Anivers�rio de Admiss�o",
                    $"Ocorreu um erro ao executar a rotina de anivers�rios de admiss�o. Erro: {ex.Message}");

                Console.WriteLine($"Erro ao executar a rotina: {ex.Message}");
            }
        }
    }
}
