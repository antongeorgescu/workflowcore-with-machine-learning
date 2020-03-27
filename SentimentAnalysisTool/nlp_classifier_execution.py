import pickle
import nltk
from nltk.tag import pos_tag
from nltk.stem.wordnet import WordNetLemmatizer
import re, string
from nltk import classify
from nltk.tokenize import word_tokenize

from flask import Flask
from flask import request,abort

app = Flask(__name__)

def remove_noise(tweet_tokens, stop_words = ()):

    cleaned_tokens = []

    for token, tag in pos_tag(tweet_tokens):
        token = re.sub('http[s]?://(?:[a-zA-Z]|[0-9]|[$-_@.&+#]|[!*\(\),]|'\
                       '(?:%[0-9a-fA-F][0-9a-fA-F]))+','', token)
        token = re.sub("(@[A-Za-z0-9_]+)","", token)

        if tag.startswith("NN"):
            pos = 'n'
        elif tag.startswith('VB'):
            pos = 'v'
        else:
            pos = 'a'

        lemmatizer = WordNetLemmatizer()
        token = lemmatizer.lemmatize(token, pos)

        if len(token) > 0 and token not in string.punctuation and token.lower() not in stop_words:
            cleaned_tokens.append(token.lower())
    return cleaned_tokens

@app.route('/')
def hello():
    return "Hello World!"

@app.route('/<name>')
def hello_name(name):
    return "Hello {}!".format(name)

@app.route('/classify',methods=['POST'])
def classify():
        
    f = open('./persistence/nbsentimentclassifier.pickle', 'rb')
    classifier = pickle.load(f)
    f.close()

    #custom_tweet = "I dunno, I feel rather crappy and depressed today. I do not see any sense looking around me.Everything is dark and discouraging."
    #custom_tokens = remove_noise(word_tokenize(custom_tweet))
    #print(classifier.classify(dict([token, True] for token in custom_tokens)))
    
    if not request.json:
        abort(400)
    message = request.json['message']
    
    custom_tokens = remove_noise(word_tokenize(message))
    result = classifier.classify(dict([token, True] for token in custom_tokens))
    return result
    
if __name__ == '__main__':
    app.run()

