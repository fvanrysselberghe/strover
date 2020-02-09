namespace Payconiq.Datamodel
{
    public class Links
    {
        public Hypermedia Self { get; set; }

        public Hypermedia Deeplink { get; set; }

        public Hypermedia Qrcode { get; set; }

        public Hypermedia Cancel { get; set; }
    }
}