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
            Console.WriteLine("Iniciando rotina de aniversários de admissão...");

            //while (true)
            //{
               
                    ExecutarRotinaAniversarios();
                //}

                //Thread.Sleep(60000);
        //    }
        }

        static void ExecutarRotinaAniversarios()
        {
            Console.WriteLine("Executando rotina de aniversários de admissão...");

            DatabaseConnection dbConnection = new DatabaseConnection();
            EmailService emailService = new EmailService();
            List<Colaborador> aniversariantes;

            try
            {
                aniversariantes = dbConnection.ObterDestinatariosHoje();

                if (aniversariantes.Count > 0)
                {
                    // Envia e-mails de aniversário para cada colaborador
                    foreach (var colaborador in aniversariantes)
                    {
                        emailService.EnviarEmailAniversario(colaborador);
                    }

                    // Envia notificação com lista de destinatários
                    string listaDestinatarios = string.Join(", ", aniversariantes.Select(c => $"{c.name} ({c.email})"));
                    emailService.EnviarEmailNotificacao("Rotina Executada com Sucesso",
                        $"A rotina de aniversários de admissão foi executada com sucesso. Destinatários:\n\n{listaDestinatarios}");

                    Console.WriteLine("E-mails enviados e notificação enviada.");
                    Console.WriteLine("E-mails enviados e notificação enviada.");
                    Console.WriteLine("E-mails enviados e notificação enviada.");
                }
                else
                {
                    // Envia notificação informando que não há aniversariantes
                    emailService.EnviarEmailNotificacao("Rotina Executada - Sem Destinatários",
                        "A rotina de aniversários de admissão foi executada, mas não havia aniversariantes hoje.");

                    Console.WriteLine("Nenhum colaborador com aniversário de admissão hoje. Notificação enviada.");
                }
            }
            catch (Exception ex)
            {
                // Envia notificação de erro
                emailService.EnviarEmailNotificacao("Erro na Rotina de Aniversário de Admissão",
                    $"Ocorreu um erro ao executar a rotina de aniversários de admissão. Erro: {ex.Message}");

                Console.WriteLine($"Erro ao executar a rotina: {ex.Message}");
            }
        }
    }
}
