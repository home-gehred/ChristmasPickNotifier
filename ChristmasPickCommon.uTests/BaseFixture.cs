using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Test
{
  public class BaseFixture
  {
    protected byte[] ConvertStringToByteArray(string data)
    {
      byte[] buffer = new byte[data.Length];
      for (int i = 0; i < data.Length; i++)
      {
        buffer[i] = (byte)data[i];
      }
      return buffer;
    }

  }
}
