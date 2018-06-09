using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioWatermark.Models
{
    public class audioSteg
    {
        private Function file;
        public audioSteg(Function file)
        {
            this.file = file;
        }

        public void waterMess(string mess)
        {
            List<short> leftStream = file.getLeftStream();
            List<short> rightStream = file.getRightStream();
            byte[] bufferMess = System.Text.Encoding.Unicode.GetBytes(mess);
            short tempBit;
            int bufferIndex = 0; 
            int bufferLength = bufferMess.Length;
            int channelLength = leftStream.Count;
            int storageBlock = (int)Math.Ceiling((double)bufferLength / (channelLength) * 2);
            leftStream[0]=(short)(bufferLength / 32767);
            rightStream[0] = (short)(bufferLength % 32767);
            for(int i = 1; i < leftStream.Count; i++)
            {
                if(i < leftStream.Count)
                    if(bufferIndex < bufferLength && i %8 > 7 - storageBlock && i % 8 <= 7)
                    {
                        tempBit = (short)bufferMess[bufferIndex++];
                        leftStream.Insert(i, tempBit);
                    }
                if(i < rightStream.Count)
                    if(bufferIndex < bufferLength && i % 8 > 7- storageBlock  && i % 8 <=7 )
                    {
                        tempBit = (short)bufferMess[bufferIndex++];
                        rightStream.Insert(i, tempBit);
                    }

            }
            file.updateStream(leftStream, rightStream);
        }
        public string extractMess()
        {
            List<short> leftStream = file.getLeftStream();
            List<short> rightStream = file.getRightStream();
            int bufferIndex = 0;
            int messageLengthQuotient = leftStream[0];
            int messageLengthRemainder = rightStream[0];
            int channelLength = leftStream.Count;
            int bufferLength = 32767 * messageLengthQuotient + messageLengthRemainder;
            int storageBlock = (int)Math.Ceiling((double)bufferLength / (channelLength * 2));
            byte[] bufferMessage = new byte[bufferLength];
            for(int i = 1; i < leftStream.Count; i++)
                if(bufferIndex <bufferLength && i % 8 > 7 - storageBlock && i % 8 <= 7)
                {
                    bufferMessage[bufferIndex++] = (byte)leftStream[i];
                    if (bufferIndex < bufferLength)
                        bufferMessage[bufferIndex++] = (byte)rightStream[i];
                }
            return System.Text.Encoding.UTF8.GetString(bufferMessage);
        }
    }
}