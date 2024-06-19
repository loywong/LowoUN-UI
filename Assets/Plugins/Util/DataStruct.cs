/// <summary>
/// author: www.loywong.com
/// std::pair this concept come from C++
/// template <class T1, class T2> struct pair;
/// pair of values
/// This class couples together a pair of values, which may be of different types (T1 and T2). The individual values can be accessed through its public members first and second.
/// Pairs are a particular case of tuple.
/// </summary>

public class Pair<T, U> {
	public T first;
	public U second;
	public Pair (T Key, U Value) {
		this.first = Key;
		this.second = Value;
	}

	public override string ToString () {
		return string.Format ("Pair:{0}+{1}", first.ToString (), second.ToString ());
	}
}
public class Tuple<T1, T2, T3> {
	public T1 item1 { get; private set; }
	public T2 item2 { get; private set; }
	public T3 item3 { get; private set; }
	public Tuple (T1 Item1, T2 Item2, T3 Item3) {
		this.item1 = Item1;
		this.item2 = Item2;
		this.item3 = Item3;
	}
}
public class Tuple<T1, T2, T3, T4> {
	public T1 item1 { get; private set; }
	public T2 item2 { get; private set; }
	public T3 item3 { get; private set; }
	public T4 item4 { get; private set; }
	public Tuple (T1 Item1, T2 Item2, T3 Item3, T4 Item4) {
		this.item1 = Item1;
		this.item2 = Item2;
		this.item3 = Item3;
		this.item4 = Item4;
	}
}
public class Tuple<T1, T2, T3, T4, T5> {
	public T1 item1 { get; private set; }
	public T2 item2 { get; private set; }
	public T3 item3 { get; private set; }
	public T4 item4 { get; private set; }
	public T5 item5 { get; private set; }
	public Tuple (T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5) {
		this.item1 = Item1;
		this.item2 = Item2;
		this.item3 = Item3;
		this.item4 = Item4;
		this.item5 = Item5;
	}
}