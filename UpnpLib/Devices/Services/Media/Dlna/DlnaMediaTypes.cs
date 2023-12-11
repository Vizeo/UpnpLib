//https://nmaier.github.io/simpleDLNA/

using System;

namespace UpnpLib.Devices.Services.Media.Dlna
{
  [Flags]
  public enum DlnaMediaTypes : int
  {
    Audio = 1 << 2,
    Image = 1 << 1,
    Video = 1 << 0,
    All = ~(-1 << 3)
  }
}
