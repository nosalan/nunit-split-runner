﻿using System.Collections.Generic;
using System.Linq;

namespace NUnitSplitRunner
{
  public class Runner
  {
    private static void Main(string[] args)
    {
      var runner = new Runner();
      runner.Run(args, 10);
    }

    public void Run(string[] args, int allowedAssemblyCount)
    {
      var processName = args[0];
      var dlls = new List<string>();
      var commandline = new CommandlineArguments();
      var chunkProcessing = new ChunkProcessing(processName, new TestChunkFactory(allowedAssemblyCount, ChunkProcessing.PartialDirName));

      Parse(args, dlls, commandline);
      chunkProcessing.Execute(dlls, commandline);
      chunkProcessing.MergeReports(ChunkProcessing.PartialDirName, ChunkProcessing.InputPattern, ChunkProcessing.OutputPath);
    }

    private static void Parse(IEnumerable<string> args, List<string> dlls, CommandlineArguments commandline)
    {
      foreach (var arg in args.Skip(1))
      {
        if (IsAssemblyPath(arg))
        {
          dlls.Add(arg);
        }
        else
        {
          commandline.Add(arg);
        }
      }
    }

    private static bool IsAssemblyPath(string arg)
    {
      return arg.EndsWith(".dll");
    }
  }
}
