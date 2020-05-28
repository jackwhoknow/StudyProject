using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class ManualResetEventSlimDemo
    {
        private const int NUM_SENTENCES = 2000000;
        private const int Timeout = 5000;
        private static ConcurrentBag<string> _sentencesBag;
        private static ConcurrentBag<string> _capWordsInSentencesBag;
        private static ConcurrentBag<string> _finalSentencesBag;

        private static ManualResetEventSlim _mResProduceSentences;
        private static ManualResetEventSlim _mResCapitalizeWords;

        public static void Run()
        {
            var sw = Stopwatch.StartNew();
            _sentencesBag = new ConcurrentBag<string>();
            _capWordsInSentencesBag = new ConcurrentBag<string>();
            _finalSentencesBag = new ConcurrentBag<string>();
            _mResProduceSentences = new ManualResetEventSlim(false, 100);
            _mResCapitalizeWords = new ManualResetEventSlim(false, 100);
            try
            {
                Parallel.Invoke(
                    () => ProduceSentences(),
                    () => CapitalizeWordsInsentences(Timeout),
                    () => RemoveLettersInSentences(Timeout)
                    );
            }
            catch (AggregateException ex)
            {
                foreach(var innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine(innerEx.Message);
                    Console.WriteLine("The producers/consumers failed their execution");
                }
            }
            finally
            {
                _mResProduceSentences.Dispose();
                _mResCapitalizeWords.Dispose();
            }
            Console.WriteLine("Number of sentences with captitalized words in the bag: {0}",_capWordsInSentencesBag.Count);
            Console.WriteLine("Number of sentences with removed letters in the bag: {0}", _finalSentencesBag.Count);
            Console.WriteLine(sw.Elapsed.ToString());
            Console.WriteLine("finished");
            Console.ReadLine();
        }

        private static void ProduceSentences()
        {
            string[] possibleSentencs = { "Lorem ipsum dolor sit amet consectetur adipisicing elit.A adipisci veniam",
                "impedit repellat ? Doloribus fugit eveniet tenetur repellendus autem, voluptatem, " ,
                "cupiditate facilis numquam deserunt, minus officia consectetur nisi ipsam hic ?",
                " Optio, autem!Aliquid distinctio natus explicabo provident ea, eos et laborum beatae,",
                " labore quidem consectetur neque hic numquam cumque quis odio vitae aspernatur vel optio"
                 };
            try
            {
                //Signal /Set
                _mResProduceSentences.Set();
                var rand = new Random();
                for (int i = 0; i < NUM_SENTENCES; i++)
                {
                    var sb = new StringBuilder();
                    for (int j = 0; j < possibleSentencs.Length; j++)
                    {
                        if (rand.Next(2) > 0)
                        {
                            sb.Append(possibleSentencs[rand.Next(possibleSentencs.Length)]);
                            sb.Append(' ');
                        }
                    }
                    if (rand.Next(20) > 15)
                    {
                        _sentencesBag.Add(sb.ToString());
                    }
                    else
                    {
                        _sentencesBag.Add(sb.ToString().ToUpper());
                    }
                }
            }
            finally
            {
                // Switch to nonsignaled/unset
                _mResProduceSentences.Reset();
            }
        }
        private const int TIMEOUT = 5000;
        private static void CapitalizeWordsInsentences(int millisecondsTimeout)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', ';', '(', ')', '[', ']', '{', '}', '/', '?', '@', '\t', '"' };
            // Start after ProduceSentences began working 
            // Wait for _mResProduceSentences to become signaled
            if(!_mResProduceSentences.Wait(millisecondsTimeout))
            {
                throw new TimeoutException(String.Format("CapitalizeWordsInSentences has been waiting "
                    + "for {0} seconds to access sentences.",millisecondsTimeout));
            }
            try
            {
                //Signal/Set
                _mResCapitalizeWords.Set();
                while ((!_sentencesBag.IsEmpty) || (_mResProduceSentences.IsSet))
                {
                    string sentence;
                    if (_sentencesBag.TryTake(out sentence))
                    {
                        _capWordsInSentencesBag.Add(CapitalizeWords(delimiterChars, sentence, '\\'));
                    }
                }
            }
            finally
            {
                //Switch to nonsignaled/unset
                _mResCapitalizeWords.Reset();
            }
        }
        private static void RemoveLettersInSentences(int millisecondsTimeout)
        {
            char[] letterChars = { 'A', 'B', 'C', 'e', 'i', 'j', 'm', 'x', 'y', 'z' };
            //Start after CapitalizeWordsInSentences began working
            // Wait for _mresCapitalizeWords to become signaled
            if (!_mResCapitalizeWords.Wait(millisecondsTimeout))
            {
                throw new TimeoutException(String.Format("RemoveLettersInSentences has been waiting "
                    + "for {0} seconds to access sentences.", millisecondsTimeout));
            }
            while ((!_capWordsInSentencesBag.IsEmpty) ||
                (_mResCapitalizeWords.IsSet)
                )
            {
                string sentence;
                if (_capWordsInSentencesBag.TryTake(out sentence))
                {
                    _finalSentencesBag.Add(RemoveLetters(letterChars, sentence));
                }
            }
        }
        private static string CapitalizeWords(char[] delimiters, string sentence, char newDelimiter)
        {
            string[] words = sentence.Split(delimiters);
            var sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 1)
                {
                    sb.Append(words[i][0].ToString().ToUpper());
                    sb.Append(words[i].Substring(1).ToLower());
                }
                else
                {
                    sb.Append(words[i].ToLower());
                }
                sb.Append(newDelimiter);
            }
            return sb.ToString();
        }
        private static string RemoveLetters(char[] letters, string sentence)
        {
            var sb = new StringBuilder();
            bool match = false;
            for (int i = 0; i < sentence.Length; i++)
            {
                for (int j = 0; j < letters.Length; j++)
                {
                    if (sentence[i] == letters[j])
                    {
                        match = true;
                        break;
                    }
                }
                if (!match)
                {
                    sb.Append(sentence[i]);
                }
                match = false;
            }
            return sb.ToString();
        }
    }
}
