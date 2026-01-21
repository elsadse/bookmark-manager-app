import { ButtonForm, FormFooterRow, FormHeader, InputField, Logo } from "@/components/connexion/FormContainerSignIn";

export function FormContainerSignUp() {

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo />
            <FormHeader text="Create your account" supportingText="Join us and start saving your favorite links — organized, searchable, and always within reach." />
            <form className="flex flex-col gap-y-4">
                <InputField labelInput="Full name *" typeInput="text" />
                <InputField labelInput="Email *" typeInput="email" />
                <InputField labelInput="Password *" typeInput="password" />
                <ButtonForm textButton="Create account" />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="Already have an account?" textFooterRowLink="Log in" linkFooterRow="/" />
            </div>
        </div>
    )
}