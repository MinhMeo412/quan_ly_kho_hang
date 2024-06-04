# Style guide

Style guide này để giúp mọi người thống nhất 1 tiêu chuẩn để nhìn cho sạch, dễ đọc, dễ hiểu. 🤓

## Mục lục

1. [Ngôn ngữ](#ngôn-ngữ)
2. [Docstrings](#docstrings)
3. [Comment](#comment)
4. [Đặt tên](#đặt-tên)

## Ngôn ngữ

Cố gắng dùng [tiếng Anh thay vì tiếng Việt](https://classroom.google.com/c/Njg0NTY0NDgzMDYy/p/Njk0MDMwMzY1ODE2/details/) khi code (đặt tên file, biến, hàm, comment, ...).
[README](../README.md), [những file markdown trong docs/](../docs/), commit message, issue, pull request thì dùng tiếng gì cũng được không quan trọng.

## Docstrings

Đọc để hiểu code khá là lâu, nên khi viết các class, function nên tóm tắt lại mục đích, tham số,... để người khác lướt qua biết dùng luôn đỡ phải hiểu code.
Ví dụ:

[C#](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments/):
```
        /**
        <summary>
            Executes a stored procedure and returns the queried table as a list of lists of objects.
        </summary>
        <param name="connectionString">Example: "server=localhost; user=root; password=1234; database=DatabaseName"</param>
        <param name="procedure">Name of the procedure to be executed.</param>
        <param name="parameters">Dictionary with parameter name as keys and their respective arguments as values.</param>
        <returns> A list containing each row of the table as a sublist.</returns>
        */
        public static List<List<object>> ExecuteReader(string connectionString, string procedure, Dictionary<string, object> parameters)
        {
            List<List<object>> rows = new List<List<object>>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(procedure, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (string parameter in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                }
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetValue(i));
                        }
                        rows.Add(row);
                    }
                }
            }
            return rows;
        }
```

MySQL:
```
/*
 * Calculate and returns the difference level between 2 strings. 
 * The closer they are to each other, the lower the return value.
 * A returned value of 0 means the strings are identical.
 * 
 * This can be used to search a value from a table without having to specify the exact value
 * Example: SELECT * FROM product_variant ORDER BY LEVENSHTEIN(color, 'orang') asc;
 * 
 * s1: string
 * s2: string
 * return: int
 */
delimiter $$
create function levenshtein( s1 varchar(255), s2 varchar(255) )
    returns int
    deterministic
    begin
        declare s1_len, s2_len, i, j, c, c_temp, cost int;
        declare s1_char char;
        -- max strlen=255
        declare cv0, cv1 varbinary(256);
        set s1_len = char_length(s1), s2_len = char_length(s2), cv1 = 0x00, j = 1, i = 1, c = 0;
        if s1 = s2 then
            return 0;
        elseif s1_len = 0 then
            return s2_len;
        elseif s2_len = 0 then
            return s1_len;
        else
            while j <= s2_len DO
                set cv1 = concat(cv1, UNHEX(hex(j))), j = j + 1;
            end while;
            while i <= s1_len DO
                set s1_char = substring(s1, i, 1), c = i, cv0 = UNHEX(hex(i)), j = 1;
                while j <= s2_len DO
                    set c = c + 1;
                    if s1_char = substring(s2, j, 1) then
                        set cost = 0; else set cost = 1;
                    end if;
                    set c_temp = conv(hex(substring(cv1, j, 1)), 16, 10) + cost;
                    if c > c_temp then set c = c_temp; end if;
                    set c_temp = conv(hex(substring(cv1, j+1, 1)), 16, 10) + 1;
                    if c > c_temp then
                        set c = c_temp;
                    end if;
                    set cv0 = concat(cv0, UNHEX(hex(c))), j = j + 1;
                end while;
                set cv1 = cv0, i = i + 1;
            end while;
        end if;
        return c;
    end$$
delimiter ;
```

# Comment

Nên ưu tiên docstring trước thay vì comment.
Với cả chưa thấy thầy nói gì về comment, nên chắc có thì tốt, không có thì cũng chả sao.
Nhưng có vẫn hơn, nên nếu được thì cứ comment, sau mình hoặc người khác đọc cho dễ hiểu.

# Đặt tên

Nhìn cho nó đẹp 😎

[C#](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names/):
    * Tên biến, tham số: camelCase (vd: string userInput, int columnCount)
    * Tên class, method, function: PascalCase (vd: public class ProductVariant(), public static int Fibonacii())
    * [...](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names/)

MySQL:
    * Tên bảng: snake_case (vd: create table engine_cost, create table time_zone), số ít (vd: user thay vì users, product thay vì products, ...)
    * Tên cột: snake_case
    * Tên function: snake_case