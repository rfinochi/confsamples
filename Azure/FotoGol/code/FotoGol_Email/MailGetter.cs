using System;
using System.Collections.Generic;

using OpenPOP.POP3;
using OpenPOP.MIMEParser;

namespace FotoGol_Email
{
    public class MailGetter
    {
        public static List<PhotoData> GetPhotoData(bool deleteMessages)
        {
            var imagesData = new List<PhotoData>();
            OpenPOP.POP3.POPClient client = new POPClient( "mail.shockbyte.com.ar", 110, "demoazure@shockbyte.com.ar", "Password11", AuthenticationMethod.USERPASS );
            Console.WriteLine ("Connected");
            var count = client.GetMessageCount( );
            Console.WriteLine ("Message Count: " + count);
            for ( int i = 1; i <= count; i++ )
            {
                Console.WriteLine ("Message");
                var message = client.GetMessage( i, false );

                foreach ( Attachment att in message.Attachments )
                {
                    Console.WriteLine ("Att: " + att.ContentFileName);
                    var extPos = att.ContentFileName.LastIndexOf( "." );
                    if ( extPos >= 0 )
                    {
                        var ext = att.ContentFileName.Substring( extPos + 1 ).ToLowerInvariant( );

                        var photo = new PhotoData( ) { Data = att.DecodedAsBytes( ), From = message.From, Title = message.Subject, FileName = att.ContentFileName };

                        switch ( ext )
                        {
                            case "jpg":
                            case "jpeg":
                                photo.ImageType = "image/jpeg";
                                imagesData.Add( photo );

                                break;

                            case "gif":
                                photo.ImageType = "image/gif";
                                imagesData.Add( photo );

                                break;

                            case "png":
                                photo.ImageType = "image/png";
                                imagesData.Add( photo );

                                break;
                        }
                    }
                }
            }

            if ( deleteMessages )
            {
                client.DeleteAllMessages( );
            }

            client.Disconnect( );

            return imagesData;
        }
    }
}