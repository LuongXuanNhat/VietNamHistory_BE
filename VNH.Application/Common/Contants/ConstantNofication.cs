using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Application.Common.Contants
{
    public static class ConstantNofication
    {
        // Post
        public static string CommentPost(string name)
        {
            return "[ "+name+" ]" + " đã bình luận một bài viết của bạn";
        }
        public static string LikePost(string name)
        {
            return "[ " + name + " ]" + " đã thích một bài viết của bạn";
        }

        // Forum
        public static string CommentAnswer(string name)
        {
            return "[ " + name + " ]" + " đã bình luận trong câu hỏi của bạn";
        }
        public static string AnswerTheQuestion(string name)
        {
            return "[ " + name + " ]" + " đã 'trả lời' một câu hỏi của bạn";
        }
        public static string LikeQuestion(string name)
        {
            return "[ " + name + " ]" + " đã tán thành một câu hỏi của bạn";
        }
        public static string LikeAnswer(string name)
        {
            return "[ " + name + " ]" + " đã 'tán thành' một câu trả lời của bạn";
        }
    }
}
