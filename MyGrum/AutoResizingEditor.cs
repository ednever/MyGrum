using Xamarin.Forms;

namespace MyGrum
{
    public class AutoResizingEditor : Editor
    {
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);
            InvalidateMeasure();
        }
    }
}