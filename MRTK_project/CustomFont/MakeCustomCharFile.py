unicode_hex_ranges = [range(0x0020, 0xFFFF)]
merge_set = set().union(*unicode_hex_ranges)

with open('MakeCustomCharFileOutput.txt', 'w', encoding='utf-8') as file:
    for codepoint in merge_set:
        char = chr(codepoint)
        try:
            file.write(char)
        except UnicodeEncodeError:
            # 忽略代理字符
            pass
