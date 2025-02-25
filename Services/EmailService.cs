using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using TunicoAniversarioAdmissao.Models;

namespace TunicoAniversarioAdmissao.Services
{
    public class EmailService
    {
        // Configuração via Variáveis de Ambiente
        private readonly string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.office365.com";
        private readonly int smtpPort = 587;
        private readonly string senderEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "";
        private readonly string senderPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "";
        private readonly string imagemParabens = Environment.GetEnvironmentVariable("IMAGEM_PARABENS") ?? "";

        public void EnviarEmailAniversario(Colaborador colaborador)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(colaborador.email);
                    mail.Subject = $"Parabéns pelo seu aniversário de admissão, {colaborador.name}!";

                    // Cópia oculta (se necessário)
                    mail.Bcc.Add(Environment.GetEnvironmentVariable("BCC_EMAIL") ?? "");

                    // Corpo do e-mail em HTML
                    string body = $@"
                        <h1>Parabéns por completar <b>{colaborador.years_on_company} ano(s)</b> na Suntrans!</h1>

                        <p>Olá {colaborador.name},</p>

                        <p>É com grande alegria que celebramos <b>{colaborador.years_on_company} ano(s)</b> de sua jornada aqui na Suntrans.</p>

                        <p>Estamos felizes em compartilhar este momento especial com você!</p>

                        {(string.IsNullOrEmpty(imagemParabens) ? "" : "<p><img src='cid:ImagemAniversario' alt='Aniversário'></p>")}

                        <p>Atenciosamente,<br><b>Equipe Suntrans</b></p>";

                    mail.IsBodyHtml = true;

                    // Adiciona imagem inline apenas se o caminho estiver configurado
                    if (!string.IsNullOrEmpty(imagemParabens))
                    {
                        var inline = new LinkedResource(imagemParabens, MediaTypeNames.Image.Jpeg)
                        {
                            ContentId = "ImagemAniversario"
                        };
                        var avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                        avHtml.LinkedResources.Add(inline);
                        mail.AlternateViews.Add(avHtml);
                    }
                    else
                    {
                        mail.Body = body;
                    }

                    // Configuração do SMTP e envio
                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                Console.WriteLine($"Email enviado para {colaborador.name} - {colaborador.email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email para {colaborador.name}: {ex.Message}");
            }
        }

        public void EnviarEmailNotificacao(string assunto, string mensagem)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(Environment.GetEnvironmentVariable("NOTIFICACAO_EMAIL") ?? "");
                    mail.Bcc.Add(Environment.GetEnvironmentVariable("BCC_EMAIL") ?? "");
                    mail.Subject = assunto;
                    mail.Body = mensagem;

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                Console.WriteLine("Notificação enviada com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar notificação: {ex.Message}");
            }
        }
    }
}
