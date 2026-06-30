using Gut.Models;

namespace Gut.Views.Templates
{
    public class SelectorTemplate : DataTemplateSelector
    {
        public DataTemplate SenderMessageTemplate { get; set; }
        public DataTemplate ReceiverMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            try
            {
                var message = (Message)item;
                if (message.Sender == null)
                    return ReceiverMessageTemplate;
                return SenderMessageTemplate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
