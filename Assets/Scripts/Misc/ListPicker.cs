using UnityEngine;
using System.Collections;
/// <summary>
/// List picker.
/// </summary>
public class ListPicker  {
	private int m_listCount;
	private bool[] m_inUse;
	private int m_count=0;
	
	
	
	public ListPicker(int count)
	{
		init (count);
	}
	public void init (int count)
	{
		m_count=count;
		m_inUse = new bool[count];
		reset();
	}
	public void reset()
	{
		m_listCount = m_count-1;
		for(int i=0; i<m_count; i++)
		{
			m_inUse[i] = false;
		}		
	}
	public int pickRandomIndex()
	{
		int rc = -1;
		if(m_count>0)
		{
			int randomIndex = Random.Range(0,m_listCount);
			rc = selectIndex( randomIndex);
			m_listCount--;
		
		
			if(rc==-1)
			{
				reset();
				rc=pickRandomIndex();
			}
		}
		return rc;
	}

	int selectIndex(int index)
	{
		int count = 0;
		int rc = -1;
		if(m_inUse==null)
		{
			return -1;
		}
		for(int i=0; i<m_inUse.Length && rc == -1; i++)
		{

			if(m_inUse[i]==false)
			{
				if(count == index)
				{
					m_inUse[i] = true;
					rc = i;
				}

				
				count++;
			}
			
		}
		return rc;
	}
}
