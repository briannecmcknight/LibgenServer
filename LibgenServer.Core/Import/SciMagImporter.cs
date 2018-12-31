﻿using System;
using System.Collections;
using System.Collections.Generic;
using LibgenServer.Core.Database;
using LibgenServer.Core.Entities;
using LibgenServer.Core.Import.SqlDump;

namespace LibgenServer.Core.Import
{
    public class SciMagImporter : Importer<SciMagArticle>
    {
        private readonly DateTime lastAddedDateTime;
        private readonly int lastModifiedLibgenId;

        public SciMagImporter(LocalDatabase localDatabase, BitArray existingLibgenIds)
            : base(localDatabase, existingLibgenIds, TableDefinitions.SciMag)
        {
            if (IsUpdateMode)
            {
                SciMagArticle lastAddedSciMagArticle = LocalDatabase.GetLastAddedSciMagArticle();
                lastAddedDateTime = lastAddedSciMagArticle.AddedDateTime ?? DateTime.MinValue;
                lastModifiedLibgenId = lastAddedSciMagArticle.LibgenId;
            }
        }

        protected override void InsertBatch(List<SciMagArticle> objectBatch)
        {
            LocalDatabase.AddSciMagArticles(objectBatch);
        }

        protected override void UpdateBatch(List<SciMagArticle> objectBatch)
        {
            throw new InvalidOperationException("Updating is not supported for articles.");
        }

        protected override bool IsNewObject(SciMagArticle importingObject)
        {
            return importingObject.AddedDateTime > lastAddedDateTime ||
                (importingObject.AddedDateTime == lastAddedDateTime && importingObject.LibgenId != lastModifiedLibgenId);
        }

        protected override int? GetExistingObjectIdByLibgenId(int libgenId)
        {
            return null;
        }
    }
}
