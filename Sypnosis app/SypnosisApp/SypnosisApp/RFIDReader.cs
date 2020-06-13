using Phidget22;
using Phidget22.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SypnosisApp
{
    class RFIDReader
    {
        RFID rfid;
        private string tagValue;
        private bool tagLost;
        bool tagConOpened = false;

        public delegate void tagScanned(string message);
        public event tagScanned tagScannedEvent;


        public void StartUp()
        {
            try
            {
                rfid = new RFID();
                rfid.Open();
                tagConOpened = true;
                rfid.Attach += Rfid_Attach;
                rfid.Tag += Rfid_Tag;
                rfid.TagLost += Rfid_TagLost;
            }
            catch (PhidgetException ex)
            {

            }
        }

        private void Rfid_Attach(object sender, AttachEventArgs e)
        {
            tagLost = false;
        }

        private void Rfid_TagLost(object sender, RFIDTagLostEventArgs e)
        {
            tagLost = true;
        }

        public string GetTagValue
        {
            get { return tagValue; }
        }

        public bool TagConOpened { get { return tagConOpened; } }

        public bool TagLost
        {
            get { return tagLost; }
        }

        private void Rfid_Tag(object sender, RFIDTagEventArgs e)
        {
            tagValue = e.Tag;
            tagScannedEvent(tagValue);
            //tagScannedEvent.Invoke(tagValue);
        }

        public void Close()
        {
            rfid.Close();
        }
    }
}