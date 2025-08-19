using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Program
    {
        public static void CallToChildThread()
        {
            Console.WriteLine("Child thread starts");
            // 线程暂停 5000 毫秒
            int sleepfor = 5000;
            Console.WriteLine("Child Thread Paused for {0} seconds",
                              sleepfor / 1000);
            Thread.Sleep(sleepfor);
            Console.WriteLine("Child thread resumes");
        }
        public static void CallToChildThread1()
        {
            try
            {
                Console.WriteLine("Child thread starts");
                // 计数到 10
                for (int counter = 0; counter <= 10; counter++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine(counter);
                }
                Console.WriteLine("Child Thread Completed");

            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort Exception");
            }
            finally
            {
                Console.WriteLine("Couldn't catch the Thread Exception");
            }
        }
        static void Main(string[] args)
        {
            //ThreadStart childref = new ThreadStart(CallToChildThread1);
            //Console.WriteLine("In Main: Creating the Child thread");
            //Thread childThread = new Thread(childref);
            //childThread.Start();
            //// 停止主线程一段时间
            ////Thread.Sleep(2000);
            //// 现在中止子线程
            //Console.WriteLine("In Main: Aborting the Child thread");
            //childThread.Abort();

            if(false)
            {
                IteratorMode(); //遍历模式
            }
            if(false)
            {
                AdpaterMode(); //适配器
            }
            #region---------- 模板方法 -----------
            //AbstractDisplay d1 = new CharDisplay('H');
            //AbstractDisplay d2 = new StringDisplay("Hello,World");
            //AbstractDisplay d3 = new StringDisplay("你好,中国");
            //d1.Display();
            //d2.Display();
            //d3.Display();
            #endregion

            #region---------- 简单工厂 -----------
            //Operation operFactory = OperationFactory.CreateOperation("+");
            //operFactory.Number1 = 100;
            //operFactory.Number2 = 200;
            //double result = operFactory.GetResult();
            #endregion

            #region---------- 策略 ---------------
            //Context context;
            //context = new Context(new ConcreteStragegyA());
            //context.ContextInterface();

            //context = new Context(new ConcreteStragegyB());
            //context.ContextInterface();

            //context = new Context(new ConcreteStragegyC());
            //context.ContextInterface();
            #endregion

            #region----------- 装饰模式 ----------
            //Person xc = new Person("小菜");
            //BigTrouser bt = new BigTrouser();
            //Tshirts tshirt = new Tshirts();

            //bt.Decorate(xc);
            //tshirt.Decorate(bt);
            //tshirt.Show();
            #endregion
            #region---------- 代理模式 ----------
            //SchoolGirl xiaoHong = new SchoolGirl { Name = "小红" };
            //Proxy proxy = new Proxy(xiaoHong);
            //proxy.GiveChocolate();
            //proxy.GiveDolls();
            //proxy.GiveFlowers();
            #endregion
            #region-----------工厂方法模式 -------------
            //IFactory operation = new AddFactory();
            //MathOperation mathOperate= operation.CreateOperation();
            //mathOperate.Number1 = 10;
            //mathOperate.Number2 = 20;
            //double result = mathOperate.GetResult();
            #endregion
            #region---------- 原型模式 -----------------
            //Resume a = new Resume("大鸟");
            //a.SetPersonalInfo(true, 27);
            //a.SetWorkExperience("1997-2000", "NBA");

            //Resume b = a.Clone() as Resume;
            //b.SetWorkExperience("1998-2006","CCTV");

            //Resume c = a.Clone() as Resume;
            //c.SetWorkExperience("1999-2004", "华为");

            //a.Display();
            //b.Display();
            //c.Display();
            #endregion
            #region----------模板方法 ------------------
            //Console.WriteLine("学生A的答案：");
            //TestPaperA studentA = new TestPaperA();
            //studentA.TestQuestion1();
            //studentA.TestQuestion2();
            //studentA.TestQuestion3();

            //Console.WriteLine("学生B的答案：");
            //TestPaperB studentB = new TestPaperB();
            //studentB.TestQuestion1();
            //studentB.TestQuestion2();
            //studentB.TestQuestion3();
            #endregion
            #region-----------建造者模式----------------
            //Form1 form1 = new Form1();
            //form1.ShowDialog();
            #endregion
            #region----------观察者模式-----------------
            //Boss lgs = new Boss();

            //StockObserver zhj = new StockObserver("张海军", lgs);
            //NBAObserver zy = new NBAObserver("张严",lgs);

            //lgs.Update += new EventHandler(zhj.CloseStockMarket);
            //lgs.Update += new EventHandler(zy.CloseNBADirectSeeding);

            //lgs.SubjectState = "你们的Boss回来了";
            //lgs.Notify();

            //Secretary secretary = new Secretary();
            //zhj = new StockObserver("张海军", secretary);
            //zy = new NBAObserver("张严", secretary);
            //secretary.Update += new EventHandler(zhj.CloseStockMarket);
            //secretary.Update += new EventHandler(zy.CloseNBADirectSeeding);

            //secretary.SubjectState = "注意：老板回来了";
            //secretary.Notify();
            #endregion
            #region----------抽象工厂模式---------------
            //User user = new User
            //{
            //    Name="AAA",
            //    Id=1000001
            //};
            //Department department = new Department
            //{
            //    Id = 1,
            //    Name = "生产部"
            //};
            //IUser userTable= DataAccess.CreateUser();
            //IDepartment departmentTable = DataAccess.CreateDepartment();
            //userTable.Insert(user);
            //userTable.GetUser(1);

            //departmentTable.Insert(department);
            //departmentTable.GetDepart(1);
            #endregion
            #region-----------观察模式------------------
            //Work emergencyProject = new Work();
            //emergencyProject.Hour = 9;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 10;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 12;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 13;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 14;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 17;

            //emergencyProject.TaskFinished = false;

            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 19;
            //emergencyProject.WriteProgram();
            //emergencyProject.Hour = 22;
            //emergencyProject.WriteProgram();
            #endregion
            #region----------适配器模式-----------------
            //Target1 target = new Adapter1();
            //target.Request();
            //Player b = new Fowards("巴蒂尔");
            //b.Attack();
            //Player m = new Guards("麦克格雷迪");
            //m.Attack();
            //Player a = new Center("YaoMing");
            //a.Attack();
            //a.Defense();

            //Player wjzf = new Translator("姚明");
            //wjzf.Attack();
            //wjzf.Defense();
            #endregion
            #region----------备忘录模式-----------------
            //Originator o = new Originator();
            //o.State = "On";
            //o.Show();

            //Caretaker c = new Caretaker();
            //c.Memento = o.CreateMemento();

            //o.State = "Off";
            //o.Show();

            //o.SetMemento(c.Memento);
            //o.Show();
            #endregion
            #region----------组合模式-------------------
            //Composite root = new Composite("root");
            //root.Add(new Leaf("Leaf A"));
            //root.Add(new Leaf("Leaf B"));

            //Composite comp = new Composite("Composite X");
            //comp.Add(new Leaf("Leaf XA"));
            //comp.Add(new Leaf("Leaf XB"));
            //root.Add(comp);

            //Composite comp1 = new Composite("Composite XY");
            //comp1.Add(new Leaf("Leaf XYA"));
            //comp1.Add(new Leaf("Leaf XYB"));
            //comp.Add(comp1);

            //root.Add(new Leaf("Leaf C"));

            //Leaf leaf = new Leaf("Leaf D");
            //root.Add(leaf);
            //root.Remove(leaf);

            //root.Display(1);
            #endregion
            #region-----------桥接模式------------------
            //Abstraction ab = new RefinedAbstraction();
            //ab.SetImplementor(new ConcreteImplementorA());
            //ab.Operation();

            //ab.SetImplementor(new ConcreteImplementorB());
            //ab.Operation();
            #endregion
            #region-----------命令模式------------------
            //开店前的准备
            //Barbecuer boy = new Barbecuer();
            //Command bakeMuttonCmd1 = new BakeMuttonCommand(boy);
            //Command bakeMuttonCmd2 = new BakeMuttonCommand(boy);
            //Command bakeChikenWingCmd1 = new BakeChickenWingCommand(boy);
            //Waiter girl = new Waiter();

            ////开门营业
            //girl.SetOrder(bakeMuttonCmd1);
            //girl.SetOrder(bakeMuttonCmd2);
            //girl.SetOrder(bakeChikenWingCmd1);

            ////点菜完毕，通知厨房
            //girl.Notify();

            //Receiver r = new Receiver(); //接收者
            //BaseCommand c = new ConcreteCommand(r); //具体命令
            //Invoker i = new Invoker(); //命令执行者
            //i.SetCommand(c); //关联命令
            //i.ExecuteCommand(); //执行命令

            #endregion
            #region-----------职责链模式----------------
            //Handler h1 = new ConcreteHandler1();
            //Handler h2 = new ConcreteHandler2();
            //Handler h3 = new ConcreteHandler3();
            ////设置职责链上家和下家
            //h1.SetSuccessor(h2);
            //h2.SetSuccessor(h3);

            //int[] requests = { 2, 5, 14, 22, 18, 3, 27, 20 };

            //foreach(int item in requests)
            //{
            //    h1.HandleRequest(item);
            //}

            //CommManager jinli = new CommManager("金利");
            //Majordomo zongjian = new Majordomo("宗剑");
            //GeneralManager zhongjingli = new GeneralManager("刘光胜");
            //jinli.SetSuperior(zongjian);
            //zongjian.SetSuperior(zhongjingli);

            //Request request1 = new Request()
            //{
            //    RequestType = "请假",
            //    RequestContent = "海军请假看病",
            //    Number = 1
            //};
            //jinli.RequestApplication(request1);

            //Request request2 = new Request()
            //{
            //    RequestType = "请假",
            //    RequestContent = "理政请假带小孩",
            //    Number = 4
            //};
            //jinli.RequestApplication(request2);

            //Request request3 = new Request()
            //{
            //    RequestType = "加薪",
            //    RequestContent = "海军请求加薪",
            //    Number = 500
            //};
            //jinli.RequestApplication(request3);

            //Request request4 = new Request()
            //{
            //    RequestType = "加薪",
            //    RequestContent = "海军请求加薪",
            //    Number = 1000
            //};
            //jinli.RequestApplication(request4);
            #endregion
            #region----------中介者模式-----------------
            //ConcreteMediator cm = new ConcreteMediator();
            //ConcreteColleague1 c1 = new ConcreteColleague1(cm);
            //ConcreteColleague2 c2 = new ConcreteColleague2(cm);

            //cm.Colleague1 = c1;
            //cm.Colleague2 = c2;

            //c1.Send("吃过饭了吗？");
            //c2.Send("没有呢，你打算请客？");

            //UnitedNationSecurityCouncil unsc = new UnitedNationSecurityCouncil();
            //USA usa = new USA(unsc);
            //Iraq iraq = new Iraq(unsc);

            //unsc.Colleague1 = usa;
            //unsc.Colleague2 = iraq;
            //usa.Declare("我要打你");
            //iraq.Declare("来吧，我等着");
            #endregion
            #region-----------享元模式------------------
            //int extrinsicstate = 22;
            //FlyWeightFactory f = new FlyWeightFactory();
            //FlyWeight fx = f.GetFlyWeight("X");
            //fx.Operation(--extrinsicstate);

            //FlyWeight fy = f.GetFlyWeight("Y");
            //fy.Operation(--extrinsicstate);

            //FlyWeight fz = f.GetFlyWeight("Z");
            //fz.Operation(--extrinsicstate);

            //FlyWeight uf = new UnSharedConcreteFlyWeight();
            //uf.Operation(--extrinsicstate);

            //WebsiteFactory wf = new WebsiteFactory();
            //Website web1 = wf.GetWebsiteCategory("产品展示");
            //web1.Use(new WebUser("ZS"));

            //Website web2 = wf.GetWebsiteCategory("产品展示");
            //web2.Use(new WebUser("LS"));

            //Website web3 = wf.GetWebsiteCategory("产品展示");
            //web3.Use(new WebUser("WW"));

            //Website web4 = wf.GetWebsiteCategory("博客");
            //web4.Use(new WebUser("ZT"));

            //Website web5 = wf.GetWebsiteCategory("博客");
            //web5.Use(new WebUser("WM"));

            //Website web6 = wf.GetWebsiteCategory("博客");
            //web6.Use(new WebUser("ZP"));

            //Console.WriteLine("网站分类总数："+wf.GetWebsiteCount());
            #endregion
            #region-----------解释器模式----------------
            //ExpressionContext context = new ExpressionContext();
            //IList<AbstractExpression> list = new List<AbstractExpression>();
            //list.Add(new TerminalExpression());
            //list.Add(new NominalExpression());
            //list.Add(new TerminalExpression());
            //list.Add(new TerminalExpression());

            //foreach(AbstractExpression exp in list)
            //{
            //    exp.Interpret(context);
            //}

            //PlayContext playContext = new PlayContext();
            //Console.WriteLine("上海滩...");
            //playContext.PlayText = "T 500 O 2 E 0.5 G 0.5 A 3 E 0.5 G 0.5 D 3 E 0.5 G 0.5 A 0.5 O 3 C 1 O 2 A 0.5 G 1 C 0.5 A 0.5 O 3 C 1 O 2 A 0.5 G 1 C 0.5 E 0.5 D 3";
            //Expression expression = null;
            //try
            //{
            //    while(playContext.PlayText.Length>0)
            //    {
            //        string str = playContext.PlayText.Substring(0, 1);
            //        switch(str)
            //        {
            //            case "O":
            //                expression = new Scale();
            //                break;
            //            case "T":
            //                expression = new Speed();
            //                break;
            //            case "C":
            //            case "D":
            //            case "E":
            //            case "F":
            //            case "G":
            //            case "A":
            //            case "B":
            //            case "P":
            //                expression = new Note();
            //                break;
            //        }
            //        expression.Interpret(playContext);
            //    }
            //}
            //catch(System.Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion
            #region-----------访问者模式----------------
            ObjectStructure os = new ObjectStructure();
            os.Attach(new Man());
            os.Attach(new Woman());

            //成功时的反应
            Success success = new Success();
            os.Display(success);

            //失败时的反应
            Failure failure = new Failure();
            os.Display(failure);

            //恋爱时的反应
            Love love = new Love();
            os.Display(love);

            ObjectStructureNew osn = new ObjectStructureNew();
            osn.Attach(new ConcreteElementA());
            osn.Attach(new ConcreteElementB());

            ConcreteVisitor1 cv1 = new ConcreteVisitor1();
            osn.Accept(cv1);

            ConcreteVisitor2 cv2 = new ConcreteVisitor2();
            osn.Accept(cv2);
            #endregion
            Console.ReadKey();
        }

        static void IteratorMode()
        {
            BookShelf bookShelf = new BookShelf();
            bookShelf.AppendBook(new Book("Around the World in 80 days"));
            bookShelf.AppendBook(new Book("Bible"));
            bookShelf.AppendBook(new Book("Cinderella"));
            bookShelf.AppendBook(new Book("Daddy-Long-Legs"));
            Iterator iterator = bookShelf.iterator();
            while (iterator.hasNext())
            {
                Book book = (Book)iterator.Next();
                Console.WriteLine(book.Name);
            }
        }

        static void AdpaterMode()
        {
            //类适配器
            IPrint print = new PrintBanner("Hello");
            print.printWeak();
            print.printStrong();

            //对象适配器
            //实例化一个适配器给目标接口
            Target target = new Adapter();
            //下面的这些就是客户端可以被识别接口了
            target.GetTemperature();
            target.GetPressure();
            target.GetHumidity();
            target.GetUltraviolet();
        }

        static void TemplateMethod()
        {
            //TODO
        }
    }
}
