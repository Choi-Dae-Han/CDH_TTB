public class ButtonData
{
    public enum  BUTTONSTATE
    {
        LOCK, SELECTEDLOCK, UNLOCK
    }
    public BUTTONSTATE buttonState;

    public ButtonData(int i)
    {
        buttonState = (BUTTONSTATE)i;
    }
}
