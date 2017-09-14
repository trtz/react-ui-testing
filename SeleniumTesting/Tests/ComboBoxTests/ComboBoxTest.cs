﻿using NUnit.Framework;

namespace SKBKontur.SeleniumTesting.Tests.ComboBoxTests
{
    public class ComboBoxTest : TestBase
    {
        public ComboBoxTest(string reactVersion, string retailUiVersion)
            : base(reactVersion, retailUiVersion)
        {
        }

        [SetUp]
        public void SetUp()
        {
            page = OpenUrl("ComboBoxes").GetPageAs<ComboBoxesTestPage>();
        }

        [Test]
        public void TestEnterAndSelectValue()
        {
            page.SimpleComboBox.InputText("Item 1");
            page.SimpleComboBox.SelectByIndex(0);
            throw new AssertionException("Неоднозначность");
            //page.SimpleComboBox.ExpectTo().TextObsolete.EqualTo("Item 1");
        }

        [Test]
        public void TestSelectMultipleItems()
        {
            page.SimpleComboBox.InputText("Item");
            page.SimpleComboBox.SelectByIndex(5);
            throw new AssertionException("Неоднозначность");
            //page.SimpleComboBox.ExpectTo().TextObsolete.EqualTo("Item 6");
        }

        [Test]
        public void TestSelectViaCustomPortalSelector()
        {
            page.SimpleComboBox.Click();
            page.SimpleComboBoxItems.ExpectTo().HaveCount(17);
        }

        private ComboBoxesTestPage page;
    }
}