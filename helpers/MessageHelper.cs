using System.Windows.Forms;

namespace iTasks.helpers
{
    public static class MessageHelper
    {
        public static void ShowError(string msg)
            => MessageBox.Show(msg, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ShowSuccess(string msg)
            => MessageBox.Show(msg, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static bool ShowConfirmation(string msg)
            => MessageBox.Show(msg, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }
}