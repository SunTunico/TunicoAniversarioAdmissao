using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using TunicoAniversarioAdmissao.Models;

namespace TunicoAniversarioAdmissao.Services
{

public class EmailService
    {
        // caminho para a imagem personalizada no servidor
        private const string imagemParabens = "";
        
        // Host da Outlook para envio de e-mails (nao alterar)
        private readonly string smtpHost = "";
        // porta de envio SMTP (nao alterar)
        private readonly int smtpPort = 587;
        // destinatario dos e-mails automaticos
        private readonly string senderEmail = "";
        private readonly string senderPassword = "";

    public void EnviarEmailAniversario(Colaborador colaborador)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(colaborador.Email);
                mail.Subject = $"Parabéns pelo seu aniversário de admissão, {colaborador.Nome}!";

                    // copia oculta
                    mail.Bcc.Add("");

                    // Caminho para a imagem no servidor
                    string caminhoImagem = imagemParabens;

                // Corpo do e-mail com HTML
                string body = $@"
                    <h1>Parabéns por completar <b>{colaborador.AnosNaEmpresa} ano(s)</b/> na Suntrans!</h1>

                    <p>Olá {colaborador.Nome},</p>

                    <p>É com grande alegria que celebramos " + $@"<b>{colaborador.AnosNaEmpresa} ano(s)</b>" + $@" de sua jornada aqui na Suntrans. Agradecemos pela dedicação, trabalho e pelos muitos aprendizados e conquistas que vieram ao longo do caminho. </p>

                    <p><b>Comprometimento, dedicação e sucesso </b> – essas são algumas das palavras que vêm à mente quando pensamos na sua trajetória com a gente. </p>
                    
                    <p>Que esta data marque o início de mais um ciclo de realizações e crescimento! Como dizem, <b>'o sucesso é a soma de pequenos esforços repetidos dia após dia', </b>e você tem demonstrado isso de forma admirável. </p> 
                    
                    <p>Estamos felizes em compartilhar este momento especial com você e desejamos que novas realizações continuem a marcar sua jornada conosco. Sua contribuição é essencial para o sucesso do nosso time, e é uma honra tê-lo(a) como parte da nossa equipe.</p>
                    
                    <p>Que os próximos capítulos sejam tão incríveis quanto sua dedicação e empenho até aqui. Vamos em frente, rumo a mais conquistas, com o mesmo amor e dedicação de sempre!</p>
    
                    <p><img src='cid:ImagemAniversario' alt='Aniversário'></p>
                    
                    <p>Atenciosamente,<br><b>Equipe Suntrans</b></p>";

                    mail.IsBodyHtml = true;

                // Cria a visualização HTML com imagem inline
                var inline = new LinkedResource(caminhoImagem, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "ImagemAniversario"
                };
                var avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                avHtml.LinkedResources.Add(inline);
                mail.AlternateViews.Add(avHtml);

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            Console.WriteLine($"Email enviado para {colaborador.Nome} - {colaborador.Email}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar email para {colaborador.Nome}: {ex.Message}");
        }
    }

        public void EnviarEmailNotificacao(string assunto, string mensagem)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(""); // Destinatario das notificações de "Sucesso / Erro"
                    mail.Bcc.Add(""); // Cópia oculta
                    mail.Bcc.Add(""); // Cópia oculta
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