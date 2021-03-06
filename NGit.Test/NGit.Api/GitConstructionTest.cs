/*
This code is derived from jgit (http://eclipse.org/jgit).
Copyright owners are documented in jgit's IP log.

This program and the accompanying materials are made available
under the terms of the Eclipse Distribution License v1.0 which
accompanies this distribution, is reproduced below, and is
available at http://www.eclipse.org/org/documents/edl-v10.php

All rights reserved.

Redistribution and use in source and binary forms, with or
without modification, are permitted provided that the following
conditions are met:

- Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the following
  disclaimer in the documentation and/or other materials provided
  with the distribution.

- Neither the name of the Eclipse Foundation, Inc. nor the
  names of its contributors may be used to endorse or promote
  products derived from this software without specific prior
  written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using NGit;
using NGit.Api;
using NGit.Errors;
using Sharpen;

namespace NGit.Api
{
	[NUnit.Framework.TestFixture]
	public class GitConstructionTest : RepositoryTestCase
	{
		private Repository bareRepo;

		/// <exception cref="System.Exception"></exception>
		[NUnit.Framework.SetUp]
		public override void SetUp()
		{
			base.SetUp();
			Git git = new Git(db);
			git.Commit().SetMessage("initial commit").Call();
			WriteTrashFile("Test.txt", "Hello world");
			git.Add().AddFilepattern("Test.txt").Call();
			git.Commit().SetMessage("Initial commit").Call();
			bareRepo = Git.CloneRepository().SetBare(true).SetURI(db.Directory.ToURI().ToString
				()).SetDirectory(CreateUniqueTestGitDir(true)).Call().GetRepository();
			AddRepoToClose(bareRepo);
		}

		/// <exception cref="System.Exception"></exception>
		[NUnit.Framework.TearDown]
		public override void TearDown()
		{
			db.Close();
			bareRepo.Close();
			base.TearDown();
		}

		[NUnit.Framework.Test]
		public virtual void TestWrap()
		{
			Git git = Git.Wrap(db);
			NUnit.Framework.Assert.AreEqual(1, git.BranchList().Call().Count);
			git = Git.Wrap(bareRepo);
			NUnit.Framework.Assert.AreEqual(1, git.BranchList().SetListMode(ListBranchCommand.ListMode
				.ALL).Call().Count);
			try
			{
				Git.Wrap(null);
				NUnit.Framework.Assert.Fail("Expected exception has not been thrown");
			}
			catch (ArgumentNullException)
			{
			}
		}

		// should not get here
		/// <exception cref="System.IO.IOException"></exception>
		[NUnit.Framework.Test]
		public virtual void TestOpen()
		{
			Git git = Git.Open(db.Directory);
			NUnit.Framework.Assert.AreEqual(1, git.BranchList().Call().Count);
			git = Git.Open(bareRepo.Directory);
			NUnit.Framework.Assert.AreEqual(1, git.BranchList().SetListMode(ListBranchCommand.ListMode
				.ALL).Call().Count);
			git = Git.Open(db.WorkTree);
			NUnit.Framework.Assert.AreEqual(1, git.BranchList().SetListMode(ListBranchCommand.ListMode
				.ALL).Call().Count);
			try
			{
				Git.Open(db.ObjectsDirectory);
				NUnit.Framework.Assert.Fail("Expected exception has not been thrown");
			}
			catch (RepositoryNotFoundException)
			{
			}
		}
		// should not get here
	}
}
