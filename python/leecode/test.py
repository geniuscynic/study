list1 = []

last = ''
for i in range(64):
    list1.append('A')
    for j in range(i+1):
        list1.append('B')
    list1.append('C')

print("".join(list1), list1[2018])