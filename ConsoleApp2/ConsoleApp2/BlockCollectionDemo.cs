using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class BlockCollectionDemo
    {
        private const int NUM_SENTENCES = 2000000;
        private static BlockingCollection<string> _sentencesBC;
        private static BlockingCollection<string> _capWordsInSentencesBC;
        private static BlockingCollection<string> _finalSentencesBC;

        private static ConcurrentBag<string> _sentencesBag;
        private static ConcurrentBag<string> _capwordsInSentencesBag;
        private static ConcurrentBag<string> _finalSentenceBag;

        private static volatile bool _producingSentences = false;
        private static volatile bool _capitalizingWords = false;
        public static void Run()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                _sentencesBag = new ConcurrentBag<string>();
                _capwordsInSentencesBag = new ConcurrentBag<string>();
                _finalSentenceBag = new ConcurrentBag<string>();

                _sentencesBC = new BlockingCollection<string>(NUM_SENTENCES);
                _capWordsInSentencesBC = new BlockingCollection<string>(NUM_SENTENCES);
                _finalSentencesBC = new BlockingCollection<string>(NUM_SENTENCES);
                _producingSentences = true;
                Parallel.Invoke(
                    () => ProduceSentences(),
                    () => CapitalizeWordsInsentences(),
                    () => RemoveLettersInSentences()
                    );
                Console.WriteLine("Number of sentences with capitalized words in the bag: {0}", _capwordsInSentencesBag.Count);
                Console.WriteLine("Number of sentences with removed letters in the bag: {0}", _finalSentenceBag.Count);
                Console.WriteLine(sw.Elapsed.ToString());
                Console.WriteLine("Finished");
                Console.ReadLine();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private static void ProduceSentences()
        {
            string[] possibleSentencs = { "Lorem ipsum dolor sit amet consectetur adipisicing elit.A adipisci veniam",
                "impedit repellat ? Doloribus fugit eveniet tenetur repellendus autem, voluptatem, " ,
                "cupiditate facilis numquam deserunt, minus officia consectetur nisi ipsam hic ?",
                " Optio, autem!Aliquid distinctio natus explicabo provident ea, eos et laborum beatae,",
                " labore quidem consectetur neque hic numquam cumque quis odio vitae aspernatur vel optio",
                "asperiores necessitatibus.Facilis laudantium a ducimus porro culpa dolore ea ? Architecto",
                " dolorem adipisci rerum ex.Saepe aliquam quasi, facere veritatis esse officiis nihil eius ?",
                " Nobis cupiditate sed earum expedita cum distinctio labore ex fuga pariatur adipisci ut reiciendis",
                " eligendi explicabo laborum, molestias porro sint officiis beatae saepe in iure dicta corporis aut",
                "cumque ? Veritatis rerum asperiores quibusdam, odio, totam repellat molestias quasi temporibus saepe",
                "architecto alias, possimus doloremque ? Deserunt, amet!Eos vitae commodi minus dolorum, quod illum ",
                "labore nesciunt doloribus vel veritatis facilis cumque fuga id est et neque consectetur amet odit",
                "optio quidem deleniti!Quod possimus asperiores magnam tempore, numquam laborum iure, temporibus pariatur,",
                "maxime hic repudiandae enim!Quam accusamus voluptatibus, reiciendis odio, soluta est alias fugit molestias",
                "at nemo consequatur aliquam exercitationem repudiandae maxime illo, autem quisquam similique magnam voluptates",
                " fuga quo.Minus, recusandae laborum eaque beatae neque officia ducimus, ex impedit, quaerat itaque ipsa ? Cumque",
                "blanditiis laborum illum iusto aut possimus qui ? Unde tenetur earum officiis obcaecati adipisci, exercitationem",
                "voluptatibus rem dolorum ea quia cum dignissimos illum deserunt eius expedita fuga temporibus eaque eos est, omnis",
                "sapiente voluptate ? Officiis nesciunt sed fugiat ut, iusto natus id tempore aspernatur numquam perspiciatis eveniet",
                " fuga quaerat quisquam cumque excepturi commodi nisi quae totam omnis at tenetur laborum voluptatibus animi praesentium.",
                "Optio quis commodi quasi non eveniet blanditiis laudantium reiciendis.Dolorem minima aperiam suscipit ? Delectus perspiciatis",
                " excepturi at minima optio amet dolores.Assumenda facilis doloribus architecto voluptatem distinctio inventore ea.Doloribus rem",
                " incidunt dicta autem magni nobis odit veritatis eius.Excepturi autem culpa impedit itaque aliquam aspernatur temporibus esse",
                " incidunt dolorem, ratione corporis laborum inventore magni qui nisi modi a natus eius, quibusdam optio eaque nesciunt!Soluta",
                " rem quaerat voluptatem voluptates fugit deleniti, quia quae et maxime consequatur nam aliquid quo expedita blanditiis temporibus",
                " modi ? Illo quod obcaecati possimus dolorem officia rerum, alias iusto et repellat iure, perferendis maiores dolore expedita culpa",
                " atque.Corporis dignissimos, non unde quod maxime alias fugiat laboriosam repudiandae illo, explicabo velit perspiciatis!Ducimus esse",
                " aspernatur repudiandae autem eos dicta vero aperiam velit quidem ? Labore cum ullam assumenda eveniet, quidem deserunt rem!Deleniti",
                " iure necessitatibus assumenda.Aspernatur, iusto!Suscipit nostrum praesentium mollitia commodi molestiae, alias nesciunt iusto, ",
                "eveniet, dolores laudantium inventore in numquam quidem sed cum possimus ? Eligendi nemo aliquid harum molestias mollitia neque ",
                "ducimus, in nihil totam quis maxime accusantium illo expedita!Libero quaerat perspiciatis asperiores explicabo autem dolor voluptas",
                " facilis perferendis cumque ? Suscipit delectus, atque voluptates eligendi illum neque veritatis alias.Aut consequatur laboriosam",
                " nisi cumque ex eveniet et excepturi officiis aliquam itaque, porro cupiditate.Molestiae totam distinctio aliquid cum, praesentium",
                " soluta, ullam fuga sint, quibusdam non laborum.In molestias nostrum iusto culpa, ducimus, dolor minus optio error, quae deleniti",
                " sequi fugit sapiente modi!Eos ipsum voluptatum soluta nesciunt sequi neque doloremque deserunt a aperiam, aspernatur culpa",
                " perspiciatis explicabo, qui nulla illo tenetur perferendis!Dolor, architecto magnam deleniti nesciunt vero sapiente iure, ",
                "impedit sequi vitae eius necessitatibus, beatae veniam molestias earum enim consequuntur obcaecati!Aspernatur dicta doloribus",
                " laudantium nam inventore nulla suscipit libero qui eos iste ipsum, laboriosam doloremque perferendis porro voluptatem, illum",
                " quam atque.Aperiam neque cum a repellat officia!Quo eum error, cumque ea saepe repellendus molestiae ipsum, doloribus necessitatibus",
                " itaque explicabo tempore totam enim odio veritatis quam quibusdam, similique ut dolorum minus temporibus aliquam eos consequatur",
                " officiis.Illum, maxime ducimus a delectus, quidem in natus, quos asperiores tempore quaerat ea ex dolorum.Quod cupiditate",
                " perferendis tenetur eligendi nemo excepturi expedita et necessitatibus cum nam sit rerum asperiores architecto ullam corrupti",
                " neque laboriosam voluptatem dolorem odit iusto unde similique hic, explicabo reiciendis ? Incidunt cupiditate beatae fugit.",
                "Culpa corporis atque ipsum suscipit voluptatum, eligendi obcaecati unde tempore amet impedit placeat consectetur, optio molestias ",
                "repellendus eveniet, quibusdam recusandae vel ? Exercitationem eveniet nobis placeat doloremque veritatis repellendus optio alias",
                " officia sit!Fuga provident fugiat maiores consectetur odio vero aut voluptas incidunt aperiam ? Cupiditate enim facilis alias ullam,",
                " modi soluta officiis cum vero quos ipsa expedita culpa, velit magnam doloremque animi saepe quaerat consequatur quasi corrupti!Aspernatur" +
                " maiores architecto fuga pariatur soluta quaerat, commodi repellat esse porro, quibusdam deleniti sint iusto labore quos nostrum incidunt" +
                " corrupti obcaecati veniam nesciunt ratione delectus.Earum ipsum asperiores laboriosam nostrum ? Veniam ipsa voluptates ad voluptatem blanditiis " +
                "eveniet dolore qui accusamus, minus consequuntur!Fuga possimus numquam vitae a sapiente, laboriosam commodi.Veniam possimus iste quo" +
                " blanditiis molestiae nulla nesciunt, quae vero ipsum quisquam labore corrupti laborum mollitia sequi necessitatibus voluptatum nihil" +
                " ipsam debitis sed voluptates, rem fuga consectetur explicabo et.Culpa doloribus deleniti provident asperiores.Molestiae neque" +
                " quibusdam placeat accusantium libero vel, modi distinctio vero doloribus ? A aliquid laboriosam est recusandae numquam, non," +
                " earum esse quasi, aperiam obcaecati optio.Minima quod excepturi nobis natus recusandae, ratione fugiat odit saepe perferendis" +
                " expedita voluptas molestias cupiditate atque provident, facilis voluptatibus neque id soluta ipsa quas sapiente enim accusamus " +
                "veritatis.Veritatis iure similique obcaecati aspernatur magni modi quos, sit minus quis eos cupiditate, est ut!In aliquam expedita " +
                "consequuntur corporis perspiciatis doloribus placeat molestias ? Porro minima impedit tempora accusantium sapiente, dolor delectus" +
                " ratione accusamus rem ? Maiores modi in doloremque accusantium recusandae animi adipisci provident voluptatum non pariatur, esse " +
                "ipsam ? Tempora, harum omnis ratione distinctio delectus eligendi impedit officia voluptatum quis veniam doloribus provident error" +
                " repudiandae eveniet expedita illo neque odio atque rem voluptatem pariatur ut exercitationem cumque vero ? Placeat praesentium mollitia " +
                "in maiores, hic ullam minus nesciunt id cupiditate iure nisi perspiciatis officia debitis dolorum, dolorem laudantium blanditiis " +
                "minima.Quam eligendi ex voluptatibus repudiandae laboriosam facere mollitia porro ad nam, aliquam, quos rem voluptates aut dolorum " +
                "tempore consectetur perspiciatis, adipisci ab sunt qui!Excepturi illo natus error, sequi quas voluptatibus illum, soluta nemo sint" +
                " quibusdam magni similique laudantium perspiciatis quis totam dolor aperiam suscipit corrupti ipsa.Eum non cum quis ?}"
                 };

            var rand = new Random();
            for(int i=0;i<NUM_SENTENCES;i++)
            {
                var sb = new StringBuilder();
                for(int j=0;j< possibleSentencs.Length;j++)
                {
                    if(rand.Next(2)>0)
                    {
                        sb.Append(possibleSentencs[rand.Next(possibleSentencs.Length)]);
                        sb.Append(' ');
                    }
                }
                if(rand.Next(20) > 15)
                {
                    _sentencesBC.Add(sb.ToString());
                }
                else
                {
                    _sentencesBC.Add(sb.ToString().ToUpper());
                }
            }
            _sentencesBC.CompleteAdding();
        }
        private static void CapitalizeWordsInsentences()
        {
            char[] delimiterChars = { ' ',',','.',':',';','(',')','[',']','{','}','/','?','@','\t','"'};
            while(!_sentencesBC.IsCompleted)
            {
                string sentence;
                if(_sentencesBC.TryTake(out sentence))
                {
                    _capWordsInSentencesBC.Add(CapitalizeWords(delimiterChars, sentence,'\\'));
                }
            }
            _capWordsInSentencesBC.CompleteAdding();
        }
        private static void RemoveLettersInSentences()
        {
            char[] letterChars = { 'A', 'B', 'C', 'D', 'e', 'i', 'j', 'm', 'x', 'y', 'z' };
            while(!_capWordsInSentencesBC.IsCompleted)
            {
                string sentence;
                if(_capWordsInSentencesBC.TryTake(out sentence))
                {
                    _finalSentencesBC.Add(RemoveLetters(letterChars,sentence));
                }
            }
        }
        private static string CapitalizeWords(char[] delimiters, string sentence, char newDelimiter)
        {
            string[] words = sentence.Split(delimiters);
            var sb = new StringBuilder();
            for (int i=0;i<words.Length;i++)
            {
                if(words[i].Length>1)
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
        private static string RemoveLetters(char[] letters,string sentence)
        {
            var sb = new StringBuilder();
            bool match = false;
            for(int i=0;i<sentence.Length;i++)
            {
                for (int j = 0; j < letters.Length; j++)
                {
                    if(sentence[i]== letters[j])
                    {
                        match = true;
                        break;
                    }
                }
                if(!match)
                {
                    sb.Append(sentence[i]);
                }
                match = false;
            }
            return sb.ToString();
        }
    }
}
