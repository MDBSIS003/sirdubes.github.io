using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class InterestResponse
    {
        private int questionId;
        private int score;
        #region Constructors
        public InterestResponse()
        {
        }

        public InterestResponse(int questionId, int score)
        {
            this.questionId = questionId;
            this.score = score;
        }

        #endregion
        public int QuestionId
        {
            get { return questionId; }

            set { questionId = value; }
        }

        public int Score
        {
            get { return score; }

            set { score = value; }
        }
    }
}
