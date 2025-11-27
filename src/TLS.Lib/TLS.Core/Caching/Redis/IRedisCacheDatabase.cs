using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Core.Caching.Redis
{
    public interface IRedisCacheDatabase
    {
        //long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None);
        //long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None);
        //RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None);
        //long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        //long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags);
        //long ListLeftPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None);
        //long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None);
        //RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None);
        //RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None);
        //long ListRightPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None);
        //long ListRightPush(RedisKey key, RedisValue[] values, CommandFlags flags);
        //long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None);
        //long ListLength(RedisKey key, CommandFlags flags = CommandFlags.None);
        //bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None);
        //bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None);
    }
}
