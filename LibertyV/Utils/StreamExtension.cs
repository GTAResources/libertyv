﻿/*
 
    LibertyV - Viewer/Editor for RAGE Package File version 7
    Copyright (C) 2013  koolk <koolkdev at gmail.com>
   
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
  
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
   
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibertyV.Utils
{
    static class StreamExtension
    {
        public static int CopyToCount(this Stream stream, Stream output, int count, IProgressReport writingProgress = null)
        {
            if (writingProgress != null)
            {
                writingProgress = new SubProgressReport(writingProgress, count);
            }
            byte[] buffer = new byte[32768];
            int start_count = count;
            int read = buffer.Length;
            int wrote = 0;
            if (read > count)
            {
                read = count;
            }
            while (read > 0 && (read = stream.Read(buffer, 0, read)) > 0)
            {
                output.Write(buffer, 0, read);
                if (writingProgress != null)
                {
                    if (writingProgress.IsCanceled())
                    {
                        throw new OperationCanceledException();
                    }
                    wrote += read;
                    writingProgress.SetProgress(wrote);
                }
                count -= read;
                read = buffer.Length;
                if (read > count)
                {
                    read = count;
                }
            }
            return start_count - count;
        }
    }
}
