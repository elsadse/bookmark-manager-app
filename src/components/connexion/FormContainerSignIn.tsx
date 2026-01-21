import logo_light from "@/assets/images/logo-light-theme.svg"

export function FormContainerSignIn() {

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo />
            <FormHeader text="Log in your account" supportingText="Welcome back! Please enter your details." />
            <form className="flex flex-col gap-y-4">
                <InputField labelInput="Email" typeInput="email" />
                <InputField labelInput="Password" typeInput="password" />
                <ButtonForm textButton="Log in" />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="Forgot password?" textFooterRowLink="Reset it" linkFooterRow="/bookmark-manager-app/forgotPassword" />
                <FormFooterRow textFooterRow="Don’t have an account?" textFooterRowLink="Sign up" linkFooterRow="/bookmark-manager-app/signUp" />
            </div>
        </div>
    )
}

export function Logo() {

    return (
        <img src={logo_light} className="h-8 w-auto flex justify-center" alt="logo" />
    )
}

export function FormHeader({ text, supportingText }: { text: string, supportingText: string }) {

    return (
        <div className="flex flex-col gap-y-1.5">
            <span className="text-preset-1">{text}</span>
            <span className="text-preset-4-md text-neutral-800">{supportingText}</span>
        </div>
    )
}

export function InputField({ typeInput, labelInput }: { typeInput: string, labelInput: string }) {

    return (
        <div className="flex flex-col gap-y-1.5">
            <span className="text-preset-4">{labelInput}</span>
            <input type={typeInput} className="p-3 rounded-8 border border-neutral-500 focus:outline-none " />
        </div>
    )
}

export function ButtonForm({ textButton }: { textButton: string }) {

    return (
        <div className="flex justify-center px-4 py-3 gap-1 bg-teal-700 rounded-8">
            <span className="text-center px-0.5 text-preset-3 text-neutral-0">{textButton}</span>
        </div>
    )
}

export function FormFooterRow({ textFooterRow, textFooterRowLink, linkFooterRow }: { textFooterRow: string, textFooterRowLink: string, linkFooterRow: string }) {

    return (
        <div className="flex flex-row gap-x-1.5">
            <span className="text-preset-4-md text-neutral-800">{textFooterRow}</span>
            <a href={linkFooterRow} className="text-preset-4">{textFooterRowLink}</a>
        </div>
    )
}