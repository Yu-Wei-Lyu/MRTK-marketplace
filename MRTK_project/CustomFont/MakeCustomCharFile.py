'''
Unicode區段查詢 https://zh.wikipedia.org/zh-tw/Unicode%E5%8D%80%E6%AE%B5
'''

unicode_hex_ranges = [
    range(0x0020, 0x007E + 1),  # 基本拉丁字母 可字化範圍 (Ascii)
    range(0x3000, 0x303F + 1),  # 中日韓符號和標點
    range(0x4E00, 0x9FFF + 1),  # 中日韓統一表意文字 (基本區)
    range(0xFF00, 0xFFEF + 1)   # 半形及全形字元
]
merge_set = set().union(*unicode_hex_ranges)
with open('MakeCustomCharFileOutput.txt', 'w', encoding='utf-8') as file:
    for codepoint in merge_set:
        char = chr(codepoint)
        file.write(char)
print("所選Unicode範圍已寫入檔案 'MakeCustomCharFileOutput.txt' 中。")
