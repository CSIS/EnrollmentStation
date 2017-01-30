namespace EnrollmentStation.Code
{
    public class YubikeyAlgorithm
    {
        public YubikeyAlgorithm(byte value, string text)
        {
            Value = value;
            Text = text;
        }

        public byte Value { get; }
        public string Text { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}