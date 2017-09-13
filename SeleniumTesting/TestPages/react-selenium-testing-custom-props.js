global.ReactSeleniumTesting = {
    attributeWhiteList: {
        'error': [/.*/],
        'disabled': [/.*/],
        'checked': [/.*/],
        'items': ['RadioGroup'],
        'value': [/.*/],
        'customProp1': ['SomeComponent'],
        'customProp2': [/^.+SomeComponent.+$/],
        'activePage': ['Paging'],
        'pagesCount': ['Paging'],
    },
    acceptAttribute: (prevAcceptResult, componentName, propName) => {
        if (componentName === 'Select' && propName === 'items') {
            return true;
        }
        return prevAcceptResult;
    }
};
