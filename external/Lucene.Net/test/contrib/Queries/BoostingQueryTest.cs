﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lucene.Net.Index;

using NUnit.Framework;

namespace Lucene.Net.Search
{
    [TestFixture]
    public class BoostingQueryTest : Lucene.Net.TestCase
    {
        [Test]
        public void TestBoostingQueryEquals()
        {
            TermQuery q1 = new TermQuery(new Term("subject:", "java"));
            TermQuery q2 = new TermQuery(new Term("subject:", "java"));
            Assert.AreEqual(q1, q2, "Two TermQueries with same attributes should be equal");
            BoostingQuery bq1 = new BoostingQuery(q1, q2, 0.1f);
            BoostingQuery bq2 = new BoostingQuery(q1, q2, 0.1f);
            Assert.AreEqual(bq1, bq2, "BoostingQuery with same attributes is not equal");
        }
    }
}
