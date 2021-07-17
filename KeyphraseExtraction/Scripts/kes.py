'''
Created on Aug 7, 2015

@author: Administrator
'''
#KE == Keyphrase extraction

import sys
import nltk
from nltk.corpus import stopwords

##### declare global (shared) variables
# ref: http://www.nltk.org/book/ch07.html#fig-ie-architecture
grammars = ['NP: {<DT>?<JJ>*<NN>}',
            'NP: {<DT>?<JJ>*<NN>*}',            
            'NP: {<DT|PP\$>?<JJ>*<NN>}',
            'NP: {<NNP>+}',
            'NP: {<NN><NN>}']
#punc_detec = nltk.data.load('tokenizers/punkt/english.pickle')


##### handle punctuation
# ref: http://www.nltk.org/api/nltk.tokenize.html
#def fn_remove_punctuation(sentence):
#    punc_detec.PUNCTUATION
#    return


##### handle np_chunk
# ref: http://www.nltk.org/book/ch07.html#noun-phrase-chunking
# ref: http://www.nltk.org/ (example on main page)
# ref: http://www.nltk.org/howto/chunk.html
# ref: http://www.eecis.udel.edu/~trnka/CISC889-11S/lectures/dongqing-chunking.pdf
# ref: http://streamhacker.com/2009/02/23/chunk-extraction-with-nltk/
def fn_np_chunk_trees(trees):
    candidates = []
    for tree in trees:
        for subtree in tree.subtrees(filter=lambda t: t.label() == 'NP'):
            # print the noun phrase as a list of part-of-speech tagged words
            termsArray = []
            for pair in subtree:                
                termsArray.append(pair[0])
                
            #print ("cand, candidate:",termsArray, candidates)
            term = ' '.join(termsArray)
            if term not in candidates:
                candidates.append(term)
                
    #print ("NP chunk:",candidates)
    return candidates

def fn_pos_tagging_sent(sentence, language):
    tokens = nltk.word_tokenize(sentence, language)   # array of token
    tagged_tokens = nltk.pos_tag(tokens)
    return tagged_tokens

def fn_np_chunk_tree_sent(tagged_tokens):
        # array of pair of token and pos-tag    
    return [nltk.RegexpParser(grammar).parse(tagged_tokens) for grammar in grammars]

def fn_np_chunk_tree_doc(document, language):
    sentences = nltk.sent_tokenize(document, language)
    return [fn_np_chunk_tree_sent(sent, language) for sent in sentences]

def fn_ngrams_sent(sentence, n, language):
    tokens = nltk.word_tokenize(sentence, language)
    lst_ngrams = list(nltk.ngrams(tokens, n))
    #print (lst_ngrams)    
    candidates = []
    
    for ngrams in lst_ngrams:
        candidates.append(' '.join(ngrams))
    return candidates

def fn_sentence_segmentation(document):
    sentences = nltk.sent_tokenize(document)
    return sentences

##### handle agrs
# ref: http://www.tutorialspoint.com/python/python_command_line_arguments.htm
def fn_handle_args():
    print ('Number of arguments:', len(sys.argv), 'arguments.')
    print ('Location:', sys.argv[0])
    return

##### execution
# ref: http://stackoverflow.com/questions/4041238/why-use-def-main
def main():        
    fn_handle_args()

    isRunning = True;

    while isRunning:
        curInput = input()
        if curInput == 'Stop':
            isRunning = False
        else:
            lst_result = []
            
            if curInput.startswith('SegmentSentences:'):
                paragraph = curInput[17:len(curInput)]
                lst_result.extend(fn_sentence_segmentation(paragraph))
                                
            if curInput.startswith('NGram'):
                # pattern: 'NGram_3:sentence'                
                for n in range(1,int(curInput[6]) + 1):          
                    sentence = curInput[8:len(curInput)]
                    lst_result.extend(fn_ngrams_sent(sentence, n, 'english'))
            
            if curInput.startswith('NPchunking:'):
                sentence = curInput[11:len(curInput)]
                taggedPoS = fn_pos_tagging_sent(sentence, 'english')
                trees = fn_np_chunk_tree_sent(taggedPoS)         
                #for tree in trees:
                    #tree.draw()       
                lst_result.extend(fn_np_chunk_trees(trees))
                
            print ('Output:', lst_result)
            if curInput.startswith('NPchunking:'):
                print ('PoS Tags:', taggedPoS)

    return


if __name__ == "__main__":
    main()
