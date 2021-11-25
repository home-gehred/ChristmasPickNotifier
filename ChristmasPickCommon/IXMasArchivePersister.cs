using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;


namespace Common.ChristmasPickList
{
  public interface IXMasArchivePersister
  {
    void SaveArchive(XMasArchive archive);
    XMasArchive LoadArchive();
  }


  public class FileArchivePersister : IXMasArchivePersister
  {
    private string mArchivePath;
    private string mArchiveBackup;

    public FileArchivePersister(string path)
    {
      mArchivePath = path;
      mArchiveBackup = string.Format("{0}.bkp", path);
    }

    public void SaveArchive(XMasArchive archive)
    {
      File.Delete(mArchiveBackup);
      if (File.Exists(mArchivePath))
      {
        File.Move(mArchivePath, mArchiveBackup);
      }
      FileStream archiveFile = new FileStream(mArchivePath, FileMode.CreateNew, FileAccess.ReadWrite);
      XmlSerializer xml = new XmlSerializer(typeof(XMasArchive));
      xml.Serialize(archiveFile, archive);
      archiveFile.Close();
    }

    public XMasArchive LoadArchive()
    {
      FileStream archiveFile = new FileStream(mArchivePath, FileMode.Open, FileAccess.Read);
      XmlSerializer xml = new XmlSerializer(typeof(XMasArchive));
      XMasArchive tmp = (XMasArchive)xml.Deserialize(archiveFile);
      archiveFile.Close();
      return tmp;
    }
  }

}
