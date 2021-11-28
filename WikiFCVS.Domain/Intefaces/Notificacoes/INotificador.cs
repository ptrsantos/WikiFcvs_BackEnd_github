using System.Collections.Generic;
using WikiFCVS.Domain.Notificacoes;

namespace WikiFCVS.Domain.Intefaces.Notificacoes
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}